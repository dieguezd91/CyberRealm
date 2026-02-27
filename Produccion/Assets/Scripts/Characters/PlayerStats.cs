using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [SerializeField] private string playerName;
    [SerializeField] private Sprite characterImage;

    [SerializeField] private int playerLevel = 1;
    [SerializeField] private int maxLevel = 50;
    
    [FormerlySerializedAs("currentXP")]
    [SerializeField] private int currentXp;
    
    [SerializeField] private int[] xpForNextLevel;
    
    [FormerlySerializedAs("baseLevelXP")]
    [SerializeField] private int baseLevelXp = 100;
    
    [SerializeField] private GameObject levelUp;

    [FormerlySerializedAs("maxHP")]
    [SerializeField] private int maxHealth = 100;
    
    [FormerlySerializedAs("currentHP")]
    [SerializeField] private int currentHealth;

    [SerializeField] private int dexterity;
    [SerializeField] private int strength;
    [SerializeField] private int defence;

    [SerializeField] private string equippedMeleeWeaponName;
    [SerializeField] private string equippedRangeWeaponName;

    [SerializeField] private int meleeDamage;
    [SerializeField] private int rangeDamage;

    [FormerlySerializedAs("equipedMeleeWeapon")]
    [SerializeField] private ItemManager equippedMeleeWeapon;
    
    [FormerlySerializedAs("equipedRangeWeapon")]
    [SerializeField] private ItemManager equippedRangeWeapon;

    [SerializeField] private FeedbackAfterCombat rewardsTexts;

    public string PlayerName => playerName;
    public Sprite CharacterImage => characterImage;
    public int PlayerLevel => playerLevel;
    public int MaxLevel => maxLevel;
    public int CurrentXp => currentXp;
    public int[] XpForNextLevel => xpForNextLevel;
    public int BaseLevelXp => baseLevelXp;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public int Dexterity => dexterity;
    public int Strength => strength;
    public int Defence => defence;
    public string EquippedMeleeWeaponName => equippedMeleeWeaponName;
    public string EquippedRangeWeaponName => equippedRangeWeaponName;
    public int MeleeDamage => meleeDamage;
    public int RangeDamage => rangeDamage;
    public ItemManager EquippedMeleeWeapon => equippedMeleeWeapon;
    public ItemManager EquippedRangeWeapon => equippedRangeWeapon;

    private void Start()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);

        xpForNextLevel = new int[maxLevel];
        xpForNextLevel[1] = baseLevelXp;

        for(int i = 2; i < xpForNextLevel.Length; i++)
        {
            xpForNextLevel[i] = (int)(0.02f * i * i * i + 3.06f * i * i + 105.6f * i);

        }
    }


    public void AddXp(int amountOfXp)
    {
        int amountToGive = Random.Range(105, 200);
        currentXp += amountToGive;
        if(currentXp > xpForNextLevel[playerLevel])
            LevelUp();
    }

    void LevelUp()
    {
        if (playerLevel % 2 == 0)
        {
            dexterity += 2;
            strength += 2;
        }
        else
        {
            defence += 2;
        }

        currentXp -= xpForNextLevel[playerLevel];
        playerLevel++;
        StartCoroutine(ShowLevelUpSign());
        StartCoroutine(rewardsTexts.ShowLifeRestored());
    }

    public void AddHealth(int amountToAdd)
    {
        currentHealth += amountToAdd;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void SetHealth(int amount)
    {
        currentHealth = amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void SetStats(int level, int xp, int hp, int str, int dex, int def)
    {
        playerLevel = level;
        currentXp = xp;
        currentHealth = hp;
        strength = str;
        dexterity = dex;
        defence = def;
    }

    public void HealFull()
    {
        currentHealth = maxHealth;
    }

    public void EquipMeleeWeapon(ItemManager meleeWeaponToEquip)
    {
        equippedMeleeWeapon = meleeWeaponToEquip;
        equippedMeleeWeaponName = equippedMeleeWeapon.ItemName;
        meleeDamage = equippedMeleeWeapon.WeaponStrength;

    }
    
    public void EquipRangeWeapon(ItemManager rangeWeaponToEquip)
    {
        equippedRangeWeapon = rangeWeaponToEquip;
        equippedRangeWeaponName = equippedRangeWeapon.ItemName;
        rangeDamage = equippedRangeWeapon.WeaponDexterity;
    }
        
    IEnumerator ShowLevelUpSign()
    {
        levelUp.SetActive(true);
        yield return new WaitForSeconds(2f);
        levelUp.SetActive(false);
    }

}
