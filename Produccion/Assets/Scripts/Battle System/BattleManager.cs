using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public bool isBattleActive;
    bool inventoryIsOpen;

    [Header("Dependencies")]
    [SerializeField] public BattleView battleView;
    private CombatSystem combatSystem;

    [Header("Scene References")]
    [SerializeField] GameObject battleScene;
    public Camera battleCamera;
    public AudioListener battleAudioListener;
    public Camera worldCamera;
    public AudioListener worldAudioListener;
    
    [SerializeField] List<BattleCharacters> activeCharacters = new List<BattleCharacters>();
    [SerializeField] GameObject lastEnemy;
    [SerializeField] GameObject enemyGO;
    Collider2D enemyCollider;

    [SerializeField] Transform playersPositions;
    [SerializeField] Transform enemiesPositions;

    [SerializeField] BattleCharacters[] playerPrefabs, enemiesPrefabs;

    [SerializeField] int currentTurn;
    [SerializeField] bool waitingForTurn;
    [SerializeField] GameObject UIButtonHolder;

    [SerializeField] BattleMoves[] battleMovesList;

    [SerializeField] float chanceToRunAway = 0.5f;
    [SerializeField] ItemsManager selectedItem;

    private int amountOfXp;

    public bool allEnemiesAreDead = true;
    public bool allPlayersAreDead = true;

    public bool randomBattle;
    public bool dinniesBattle;
    public bool bossBattle;

    public event EventHandler OnBattleEnd;

    AudioSource combatSong;
    RandomBattle randomCombat;
    float lastRandomBattle;
    [SerializeField] float inmunityTime;

    void Awake()
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
        worldCamera = PlayerController.instance.worldCamera.GetComponent<Camera>();
        worldAudioListener = worldCamera.gameObject.GetComponent<AudioListener>();
        battleAudioListener = battleCamera.gameObject.GetComponent<AudioListener>();
        combatSong = MusicManager.instance.GetComponent<AudioSource>();
        lastRandomBattle = 0;
    }

    void Update()
    {
        CheckPlayerButtonHolder();
    }

    private void CheckPlayerButtonHolder()
    {
        if (isBattleActive)
        {
            if (waitingForTurn)
            {
                if (activeCharacters[currentTurn].IsPlayer())
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

    public void StartBattle(GameObject enemy, string enemiesToSpawn, bool isDinniesBAttle, bool isRandomBattle, bool isBossBattle)
    {
        randomBattle = isRandomBattle;
        dinniesBattle = isDinniesBAttle;
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
            AddingEnemies(enemiesToSpawn);
            
            battleView.UpdatePlayerStats(activeCharacters);
            battleView.UpdateEnemyStats(activeCharacters);
            
            if (activeCharacters.Count > 0 && activeCharacters[0].IsPlayer())
            {
                CheckAmmoStatus(activeCharacters[0].equipedRangeWeapon);
                battleView.UpdateAmmo(activeCharacters[0].equipedRangeWeapon);
            }

            waitingForTurn = true;
            currentTurn = 0;
        }
        GameManager.instance.player.SetActive(false);
    }

    private void AddingEnemies(string enemiesToSpawn)
    {
        for (int j = 0; j < enemiesPrefabs.Length; j++)
        {
            if (enemiesPrefabs[j].characterName == enemiesToSpawn)
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
                    if (playerPrefabs[j].characterName == GameManager.instance.GetPlayerStats()[i].playerName)
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

        activeCharacters[i].currentHP = player.currentHP;
        activeCharacters[i].maxHP = player.maxHP;

        activeCharacters[i].level = player.playerLevel;

        activeCharacters[i].dexterity = player.dexterity;
        activeCharacters[i].strength = player.strength;
        activeCharacters[i].defence = player.defence;

        activeCharacters[i].meleeWeaponDamage = player.meleeDamage;
        activeCharacters[i].rangeWeaponDamage = player.rangeDamage;

        if (i == 0 && player.equipedRangeWeapon != null)
        {
            activeCharacters[i].equipedRangeWeapon = player.equipedRangeWeapon;
        }
    }

    private void ExportPlayerStats(int i)
    {
        PlayerStats player = GameManager.instance.GetPlayerStats()[i];

        player.currentHP = activeCharacters[i].currentHP;
        if (PlayerStats.instance.playerLevel > activeCharacters[i].level)
            PlayerStats.instance.currentHP = PlayerStats.instance.maxHP;
    }

    private void SettingUpBattle()
    {
        isBattleActive = true;
        GameManager.instance.battleIsActive = true;

        battleScene.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        battleCamera.gameObject.SetActive(true);
        worldAudioListener.enabled = false;
        battleAudioListener.enabled = true;
        MusicManager.instance.audioSource.Stop();
        combatSong.clip = MusicManager.instance.songs[4];
        MusicManager.instance.audioSource.Play();
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
        bool allEnemiesAreDead = true;
        bool allPlayersAreDead = true;

        int ammoRewards = UnityEngine.Random.Range(3, 5);        

        for (int i = 0; i < activeCharacters.Count; i++)
        {
            if (activeCharacters[i].currentHP < 0)
            {
                activeCharacters[i].currentHP = 0;
            }

            if (activeCharacters[i].currentHP == 0)
            {
                //kill character
            }
            else
            {
                if (activeCharacters[i].IsPlayer())
                    allPlayersAreDead = false;
                else
                    allEnemiesAreDead = false;
            }
        }

        if (allEnemiesAreDead || allPlayersAreDead)
        {
            GameManager.instance.player.SetActive(true);

            if (allEnemiesAreDead)
            {
                PlayerStats.instance.AddXP(amountOfXp);
                MenuManager.instance.AddCreditsUI();
                Inventory.instance.pistolAmmo += ammoRewards;
                StartCoroutine(battleView.rewardsTexts.ShowAmmoRewards(ammoRewards.ToString()));
                ExportPlayerStats(0);
                if (!randomBattle) Destroy(enemyGO);
                
                if (randomBattle || dinniesBattle)
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
            while (activeCharacters[currentTurn].currentHP == 0)
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
        
        switch (activeCharacters[0].equipedRangeWeapon.itemName)
        {
            case "Pistola":
                Inventory.instance.pistolAmmo--;
                break;
            case "Subfusil":
                Inventory.instance.SMGAmmo--;
                break;
            case "Escopeta":
                Inventory.instance.shotgunAmmo--;
                break;
        }
        
        CheckAmmoStatus(activeCharacters[0].equipedRangeWeapon);
        battleView.UpdateAmmo(activeCharacters[0].equipedRangeWeapon);
        AudioManager.instance.selectRangeAttackSFX(activeCharacters[0].equipedRangeWeapon);

        NextTurn();
    }

    public void PlayerMeleeAttack()
    {
        StartCoroutine(battleView.Shake(activeCharacters[1].GetComponent<Rigidbody2D>()));
        DealMeleeDamageToCharacters(1);

        if (activeCharacters[0].equipedRangeWeapon == null) 
            AudioManager.instance.SelectMeleeAttackSFX(null);
        else 
            AudioManager.instance.SelectMeleeAttackSFX(activeCharacters[0].equipedMeleeWeapon);

        NextTurn();
    }

    private void DealRangeDamageToCharacters(int selectedCharacterToAttack)
    {
        int damageToGive = combatSystem.CalculateRangeDamage(activeCharacters[currentTurn], activeCharacters[selectedCharacterToAttack]);
        
        string attackerName = activeCharacters[currentTurn].characterName;
        string defenderName = activeCharacters[selectedCharacterToAttack].characterName;

        StartCoroutine(battleView.ShowLog($"{attackerName} usa ataque a rango y causa {damageToGive} de dano a {defenderName}"));

        bool isPlayerAttacking = currentTurn == 0;
        StartCoroutine(battleView.ShowDamageEffect(damageToGive, false, isPlayerAttacking));
        
        activeCharacters[selectedCharacterToAttack].TakeHPDamage(damageToGive);
    }

    private void DealMeleeDamageToCharacters(int selectedCharacterToAttack)
    {
        int damageToGive = combatSystem.CalculateMeleeDamage(activeCharacters[currentTurn], activeCharacters[selectedCharacterToAttack]);

        string attackerName = activeCharacters[currentTurn].characterName;
        string defenderName = activeCharacters[selectedCharacterToAttack].characterName;

        StartCoroutine(battleView.ShowLog($"{attackerName} usa ataque melee y causa {damageToGive} de dano a {defenderName}"));

        bool isPlayerAttacking = currentTurn == 0;
        StartCoroutine(battleView.ShowDamageEffect(damageToGive, false, isPlayerAttacking));
        
        activeCharacters[selectedCharacterToAttack].TakeHPDamage(damageToGive);
    }

    private void Heal()
    {
        activeCharacters[currentTurn].AddHP(50);
        StartCoroutine(battleView.ShowLog($"{activeCharacters[currentTurn].name} se cura 50 puntos de vida."));
        
        bool isPlayerAttacking = currentTurn == 0;
        StartCoroutine(battleView.ShowDamageEffect(50, true, isPlayerAttacking));
    }

    public void RunAway()
    {
        if (UnityEngine.Random.value > chanceToRunAway)
        {
            NextTurn();
            StartCoroutine(ScapingTime());
            if(randomBattle || dinniesBattle)
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

        foreach (ItemsManager item in Inventory.instance.GetItemsList())
        {
            battleView.CreateItemButton(item);
        }
    }

    public void SelectedItemToUse(ItemsManager itemToUse)
    {
        selectedItem = itemToUse;
        battleView.SelectItem(itemToUse);
    }

    public void UseItemButton(int selectedPlayer)
    {
        activeCharacters[selectedPlayer].UseItemInBattle(selectedItem);
        Inventory.instance.RemoveItem(selectedItem);
        
        StartCoroutine(battleView.ShowLog($"{activeCharacters[currentTurn].characterName} usa {selectedItem.name} y se cura {selectedItem.amountOfAffect} puntos de vida."));
        
        bool isPlayerAttacking = currentTurn == 0;
        StartCoroutine(battleView.ShowDamageEffect(selectedItem.amountOfAffect, true, isPlayerAttacking));
        
        battleView.UpdatePlayerStats(activeCharacters);
        UpdateItemsInInventory();
        battleView.ShowItemsMenu(false);
        
        if (activeCharacters[0].IsPlayer())
        {
            CheckAmmoStatus(activeCharacters[0].equipedRangeWeapon);
            battleView.UpdateAmmo(activeCharacters[0].equipedRangeWeapon);
        }

        NextTurn();
    }

    private void CheckAmmoStatus(ItemsManager equipedRangeWeapon)
    {
        if (equipedRangeWeapon != null)
        {
            switch (equipedRangeWeapon.itemName)
            {
                case "Escopeta":
                    Inventory.instance.hasAmmo = Inventory.instance.shotgunAmmo > 0;
                    break;
                case "Subfusil":
                    Inventory.instance.hasAmmo = Inventory.instance.SMGAmmo > 0;
                    break;
                case "Pistola":
                    Inventory.instance.hasAmmo = Inventory.instance.pistolAmmo > 0;
                    break;
            }
        }
        else
        {
            Inventory.instance.hasAmmo = false;
        }
    }

    IEnumerator ScapingTime()
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
        GameManager.instance.player.SetActive(true);
        GameManager.instance.battleIsActive = false;
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
        combatSong.clip = MusicManager.instance.activeClip;
        combatSong.Play();
    }
}
