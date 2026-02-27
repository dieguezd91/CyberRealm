using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    
    [SerializeField] private bool isBattleActive;
    private bool inventoryIsOpen;

    [Header("Dependencies")]
    [SerializeField] private BattleView battleView;
    private CombatSystem combatSystem;

    [Header("Scene References")]
    [SerializeField] private GameObject battleScene;
    [SerializeField] private Camera battleCamera;
    [SerializeField] private AudioListener battleAudioListener;
    [SerializeField] private Camera worldCamera;
    [SerializeField] private AudioListener worldAudioListener;
    
    [SerializeField] private List<BattleCharacters> activeCharacters = new List<BattleCharacters>();
    [SerializeField] private GameObject lastEnemy;
    [SerializeField] private GameObject enemyGO;
    private Collider2D enemyCollider;

    [SerializeField] private Transform playersPositions;
    [SerializeField] private Transform enemiesPositions;

    [SerializeField] private BattleCharacters[] playerPrefabs;
    [SerializeField] private BattleCharacters[] enemiesPrefabs;

    [SerializeField] private int currentTurn;
    [SerializeField] private bool waitingForTurn;
    [SerializeField] private GameObject UIButtonHolder;

    [SerializeField] private BattleMoves[] battleMovesList;

    [SerializeField] private float chanceToRunAway = 0.5f;
    [SerializeField] private ItemManager selectedItem;

    private int amountOfXp;

    [SerializeField] private bool allEnemiesAreDead = true;
    [SerializeField] private bool allPlayersAreDead = true;

    [SerializeField] private bool randomBattle;
    
    [FormerlySerializedAs("dinniesBattle")]
    [SerializeField] private bool isDinnieBattle;
    
    [SerializeField] private bool bossBattle;

    public event EventHandler OnBattleEnd;

    private AudioSource combatSong;
    private RandomBattle randomCombat;
    private float lastRandomBattle;
    [SerializeField] private float inmunityTime;

    public bool IsBattleActive => isBattleActive;
    public bool IsDinnieBattle => isDinnieBattle;
    public bool BossBattle => bossBattle;
    public bool RandomBattle => randomBattle;
    public BattleView View => battleView;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
        combatSystem = new CombatSystem();
    }

    private void Start()
    {
        worldCamera = PlayerController.instance.WorldCamera.GetComponent<Camera>();
        worldAudioListener = worldCamera.gameObject.GetComponent<AudioListener>();
        battleAudioListener = battleCamera.gameObject.GetComponent<AudioListener>();
        combatSong = MusicManager.instance.GetComponent<AudioSource>();
        lastRandomBattle = 0;
    }

    private void Update()
    {
        CheckPlayerButtonHolder();
    }

    private void CheckPlayerButtonHolder()
    {
        if (isBattleActive)
        {
            if (waitingForTurn)
            {
                if (activeCharacters[currentTurn].IsPlayer)
                {
                    UIButtonHolder.SetActive(true);
                }
                else
                {
                    UIButtonHolder.SetActive(false);
                    StartCoroutine(EnemyMoveCoroutine());
                }
            }
        }
    }

    public void StartBattle(GameObject enemy, EnemyType enemyToSpawn, bool isDinnieBattleParam, bool isRandomBattle, bool isBossBattle)
    {
        randomBattle = isRandomBattle;
        isDinnieBattle = isDinnieBattleParam;
        bossBattle = isBossBattle;

        if (randomBattle)
        {
            if (Time.time >= lastRandomBattle + inmunityTime)
                randomCombat = enemy.GetComponent<RandomBattle>();
            else
                return;
        }
        
        Destroy(lastEnemy);
        if (enemy != null)
        {
            enemyGO = enemy;
            enemyCollider = enemyGO.GetComponent<Collider2D>();
        }

        StartCoroutine(battleView.ShowLog(string.Empty));

        if (!isBattleActive)
        {
            SettingUpBattle();
            AddingPlayers();
            AddingEnemies(enemyToSpawn);
            
            battleView.UpdatePlayerStats(activeCharacters);
            battleView.UpdateEnemyStats(activeCharacters);
            
            if (activeCharacters.Count > 0 && activeCharacters[0].IsPlayer)
            {
                CheckAmmoStatus(activeCharacters[0].EquippedRangeWeapon);
                battleView.UpdateAmmo(activeCharacters[0].EquippedRangeWeapon);
            }

            waitingForTurn = true;
            currentTurn = 0;
        }
        GameManager.instance.Player.SetActive(false);
    }

    private void AddingEnemies(EnemyType enemyToSpawn)
    {
        for (int j = 0; j < enemiesPrefabs.Length; j++)
        {
            EnemyType prefabEnemyType = enemiesPrefabs[j].EnemyType;
            if (prefabEnemyType == EnemyType.None)
            {
                prefabEnemyType = CombatEnumAdapter.GetEnemyType(enemiesPrefabs[j].CharacterName);
            }

            if (prefabEnemyType == enemyToSpawn)
            {
                BattleCharacters newEnemy = Instantiate(
                    enemiesPrefabs[j],
                    enemiesPositions.position,
                    enemiesPositions.rotation,
                    enemiesPositions
                    );
                if (activeCharacters.Count == 1)
                    activeCharacters.Add(newEnemy);
                else
                    activeCharacters[1] = newEnemy;
                lastEnemy = activeCharacters[1].gameObject;
                break;
            }
        }
    }

    private void AddingPlayers()
    {
        if (activeCharacters.Count > 0)
            Destroy(activeCharacters[0].gameObject);
            
        for (int i = 0; i < GameManager.instance.GetPlayerStats().Length; i++)
        {
            if (GameManager.instance.GetPlayerStats()[i].gameObject.activeInHierarchy)
            {
                for (int j = 0; j < playerPrefabs.Length; j++)
                {
                    if (playerPrefabs[j].CharacterName == GameManager.instance.GetPlayerStats()[i].PlayerName)
                    {
                        BattleCharacters newPlayer = Instantiate(
                            playerPrefabs[j],
                            playersPositions.position,
                            playersPositions.rotation,
                            playersPositions
                            );

                        if (activeCharacters.Count == 0)
                            activeCharacters.Add(newPlayer);
                        else
                            activeCharacters[0] = newPlayer;
                        ImportPlayerStats(i);
                    }
                }
            }
        }
    }

    private void ImportPlayerStats(int i)
    {
        PlayerStats player = GameManager.instance.GetPlayerStats()[i];
        activeCharacters[i].SetStats(player.CurrentHealth, player.MaxHealth, player.PlayerLevel, player.Dexterity, player.Strength, player.Defence, player.MeleeDamage, player.RangeDamage);
        activeCharacters[i].SetEquippedWeapons(player.EquippedMeleeWeapon, player.EquippedRangeWeapon);
    }

    private void ExportPlayerStats(int i)
    {
        PlayerStats player = GameManager.instance.GetPlayerStats()[i];
        player.SetHealth(activeCharacters[i].CurrentHealth);
        if (PlayerStats.instance.PlayerLevel > activeCharacters[i].Level)
            PlayerStats.instance.HealFull();
    }

    private void SettingUpBattle()
    {
        isBattleActive = true;
        GameManager.instance.BattleIsActive = true;

        battleScene.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        battleCamera.gameObject.SetActive(true);
        worldAudioListener.enabled = false;
        battleAudioListener.enabled = true;
        MusicManager.instance.AudioSource.Stop();
        combatSong.clip = MusicManager.instance.Songs[4];
        MusicManager.instance.AudioSource.Play();
    }

    private void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeCharacters.Count)
            currentTurn = 0;

        waitingForTurn = true;
        UpdateBattle();
        battleView.UpdatePlayerStats(activeCharacters);
        battleView.UpdateEnemyStats(activeCharacters);
    }

    private void UpdateBattle()
    {
        allEnemiesAreDead = true;
        allPlayersAreDead = true;

        int ammoRewards = UnityEngine.Random.Range(3, 5);        

        for (int i = 0; i < activeCharacters.Count; i++)
        {
            if (activeCharacters[i].CurrentHealth > 0)
            {
                if (activeCharacters[i].IsPlayer)
                    allPlayersAreDead = false;
                else
                    allEnemiesAreDead = false;
            }
        }

        if (allEnemiesAreDead || allPlayersAreDead)
        {
            GameManager.instance.Player.SetActive(true);

            if (allEnemiesAreDead)
            {
                PlayerStats.instance.AddXp(amountOfXp);
                MenuManager.instance.AddCreditsUI();
                Inventory.instance.PistolAmmo += ammoRewards;
                StartCoroutine(battleView.rewardsTexts.ShowAmmoRewards(ammoRewards.ToString()));
                ExportPlayerStats(0);
                if (!randomBattle) Destroy(enemyGO);
                
                if (randomBattle || IsDinnieBattle)
                    OnBattleEnd?.Invoke(this, EventArgs.Empty);
                    
                if(bossBattle)
                    StartCoroutine(battleView.rewardsTexts.ShowLifeRestored());
            }
            else if (allPlayersAreDead)
            {
                ExportPlayerStats(0);
                GameManager.instance.RespawnPlayer();
            }

            EndBattle();
        }
        else
        {
            while (activeCharacters[currentTurn].CurrentHealth == 0)
            {
                currentTurn++;
                if (currentTurn >= activeCharacters.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    public IEnumerator EnemyMoveCoroutine()
    {
        waitingForTurn = false;

        yield return new WaitForSeconds(1f);
        EnemyAttack();

        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    private void EnemyAttack()
    {
        StartCoroutine(battleView.Shake(activeCharacters[0].GetComponent<Rigidbody2D>()));
        
        int selectedPlayerToAttack = combatSystem.SelectRandomPlayerTarget(activeCharacters);
        if (selectedPlayerToAttack == -1) return;

        AttackType attackType = combatSystem.DecideEnemyAttack(activeCharacters[currentTurn]);

        switch (attackType)
        {
            case AttackType.Heal:
                Heal();
                break;
            case AttackType.Range:
                DealRangeDamageToCharacters(selectedPlayerToAttack);
                break;
            case AttackType.Melee:
                DealMeleeDamageToCharacters(selectedPlayerToAttack);
                break;
        }

        battleView.UpdatePlayerStats(activeCharacters);
    }

    public void PlayerRangeAttack()
    {
        StartCoroutine(battleView.Shake(activeCharacters[1].GetComponent<Rigidbody2D>()));
        DealRangeDamageToCharacters(1);
        
        switch (activeCharacters[0].EquippedRangeWeapon.WeaponType)
        {
            case WeaponType.Pistola:
                Inventory.instance.PistolAmmo--;
                break;
            case WeaponType.Subfusil:
                Inventory.instance.SmgAmmo--;
                break;
            case WeaponType.Escopeta:
                Inventory.instance.ShotgunAmmo--;
                break;
        }
        
        CheckAmmoStatus(activeCharacters[0].EquippedRangeWeapon);
        battleView.UpdateAmmo(activeCharacters[0].EquippedRangeWeapon);
        AudioManager.instance.SelectRangeAttackSfx(activeCharacters[0].EquippedRangeWeapon);

        NextTurn();
    }

    public void PlayerMeleeAttack()
    {
        StartCoroutine(battleView.Shake(activeCharacters[1].GetComponent<Rigidbody2D>()));
        DealMeleeDamageToCharacters(1);

        if (activeCharacters[0].EquippedRangeWeapon == null) 
            AudioManager.instance.SelectMeleeAttackSfx(null);
        else 
            AudioManager.instance.SelectMeleeAttackSfx(activeCharacters[0].EquippedMeleeWeapon);

        NextTurn();
    }

    private void DealRangeDamageToCharacters(int selectedCharacterToAttack)
    {
        int damageToGive = combatSystem.CalculateRangeDamage(activeCharacters[currentTurn], activeCharacters[selectedCharacterToAttack]);
        
        string attackerName = activeCharacters[currentTurn].CharacterName;
        string defenderName = activeCharacters[selectedCharacterToAttack].CharacterName;

        StartCoroutine(battleView.ShowLog($"{attackerName} usa ataque a rango y causa {damageToGive} de dano a {defenderName}"));

        bool isPlayerAttacking = currentTurn == 0;
        StartCoroutine(battleView.ShowDamageEffect(damageToGive, false, isPlayerAttacking));
        
        activeCharacters[selectedCharacterToAttack].TakeDamage(damageToGive);
    }

    private void DealMeleeDamageToCharacters(int selectedCharacterToAttack)
    {
        int damageToGive = combatSystem.CalculateMeleeDamage(activeCharacters[currentTurn], activeCharacters[selectedCharacterToAttack]);

        string attackerName = activeCharacters[currentTurn].CharacterName;
        string defenderName = activeCharacters[selectedCharacterToAttack].CharacterName;

        StartCoroutine(battleView.ShowLog($"{attackerName} usa ataque melee y causa {damageToGive} de dano a {defenderName}"));

        bool isPlayerAttacking = currentTurn == 0;
        StartCoroutine(battleView.ShowDamageEffect(damageToGive, false, isPlayerAttacking));
        
        activeCharacters[selectedCharacterToAttack].TakeDamage(damageToGive);
    }

    private void Heal()
    {
        activeCharacters[currentTurn].AddHealth(50);
        StartCoroutine(battleView.ShowLog($"{activeCharacters[currentTurn].CharacterName} heals 50 health points."));
        
        bool isPlayerAttacking = currentTurn == 0;
        StartCoroutine(battleView.ShowDamageEffect(50, true, isPlayerAttacking));
    }

    public void RunAway()
    {
        if (UnityEngine.Random.value > chanceToRunAway)
        {
            NextTurn();
            StartCoroutine(ScapingTime());
            if(randomBattle || IsDinnieBattle)
                OnBattleEnd?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            StartCoroutine(battleView.ShowLog("Intentas escapar pero fallas."));
            NextTurn();
        }
        ExportPlayerStats(0);
    }

    public void UpdateItemsInInventory()
    {
        if (!inventoryIsOpen && isBattleActive)
            battleView.ShowItemsMenu(true);
            
        inventoryIsOpen = !inventoryIsOpen;
        
        battleView.ClearItemsMenu();

        foreach (ItemManager item in Inventory.instance.GetItemsList())
        {
            battleView.CreateItemButton(item);
        }
    }

    public void SelectedItemToUse(ItemManager itemToUse)
    {
        selectedItem = itemToUse;
        battleView.SelectItem(itemToUse);
    }

    public void UseItemButton(int selectedPlayer)
    {
        activeCharacters[selectedPlayer].UseItemInBattle(selectedItem);
        Inventory.instance.RemoveItem(selectedItem);
        
        StartCoroutine(battleView.ShowLog($"{activeCharacters[currentTurn].CharacterName} uses {selectedItem.ItemName} and heals {selectedItem.AmountOfAffect} health points."));
        
        bool isPlayerAttacking = currentTurn == 0;
        StartCoroutine(battleView.ShowDamageEffect(selectedItem.AmountOfAffect, true, isPlayerAttacking));
        
        battleView.UpdatePlayerStats(activeCharacters);
        UpdateItemsInInventory();
        battleView.ShowItemsMenu(false);
        
        if (activeCharacters[0].IsPlayer)
        {
            CheckAmmoStatus(activeCharacters[0].EquippedRangeWeapon);
            battleView.UpdateAmmo(activeCharacters[0].EquippedRangeWeapon);
        }

        NextTurn();
    }

    private void CheckAmmoStatus(ItemManager equipedRangeWeapon)
    {
        if (equipedRangeWeapon != null)
        {
            switch (equipedRangeWeapon.WeaponType)
            {
                case WeaponType.Escopeta:
                    Inventory.instance.HasAmmo = Inventory.instance.ShotgunAmmo > 0;
                    break;
                case WeaponType.Subfusil:
                    Inventory.instance.HasAmmo = Inventory.instance.SmgAmmo > 0;
                    break;
                case WeaponType.Pistola:
                    Inventory.instance.HasAmmo = Inventory.instance.PistolAmmo > 0;
                    break;
            }
        }
        else
        {
            Inventory.instance.HasAmmo = false;
        }
    }

    private IEnumerator ScapingTime()
    {
        if (!randomBattle) enemyCollider.enabled = false;
        StartCoroutine(battleView.ShowLog("Intentas escapar y lo logras."));
        yield return new WaitForSeconds(2f);
        EndBattle();
        yield return new WaitForSeconds(3f);
        if (!randomBattle) enemyCollider.enabled = true;
    }

    private void EndBattle()
    {
        GameManager.instance.Player.SetActive(true);
        GameManager.instance.BattleIsActive = false;
        isBattleActive = false;
        worldCamera.gameObject.SetActive(true);
        battleCamera.gameObject.SetActive(false);
        worldAudioListener.enabled = true;
        battleAudioListener.enabled = false;
        battleScene.SetActive(false);
        if (randomBattle)
        {
            lastRandomBattle = Time.time;
            StartCoroutine(randomCombat.Inmunity());
        }        
        combatSong.Stop();
        combatSong.clip = MusicManager.instance.ActiveClip;
        combatSong.Play();
    }
}
