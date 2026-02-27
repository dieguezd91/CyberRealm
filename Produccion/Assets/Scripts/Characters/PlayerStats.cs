using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [SerializeField] public string playerName;
    [SerializeField] public Sprite characterImage;

    [SerializeField] public int playerLevel = 1;
    [SerializeField] public int maxLevel = 50;
    [SerializeField] public int currentXP;
    [SerializeField] public int[] xpForNextLevel;
    [SerializeField] public int baseLevelXP = 100;
    public GameObject levelUp;

    [SerializeField] public int maxHP = 100;
    [SerializeField] public int currentHP;

    public int dexterity;
    public int strength;
    public int defence;

    public string equippedMeleeWeaponName;
    public string equippedRangeWeaponName;

    public int meleeDamage;
    public int rangeDamage;

    public ItemsManager equipedMeleeWeapon, equipedRangeWeapon;

    [SerializeField] FeedbackAfterCombat rewardsTexts;

    private void Start()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);

        xpForNextLevel = new int[maxLevel];
        xpForNextLevel[1] = baseLevelXP;

        for(int i = 2; i < xpForNextLevel.Length; i++)
        {
            xpForNextLevel[i] = (int)(0.02f * i * i * i + 3.06f * i * i + 105.6f * i);

        }
    }


    public void AddXP(int amountOfXp)
    {
        int amountToGive = Random.Range(105, 200);
        Debug.Log(currentXP);
        Debug.Log(amountOfXp);
        currentXP += amountToGive;
        if(currentXP > xpForNextLevel[playerLevel])
            LevelUp();

        Debug.Log(currentXP);
    }

    void LevelUp()
    {
        Debug.Log(playerLevel);
        if (playerLevel % 2 == 0)
        {
            Debug.Log("Gain DXT and STRG");
            dexterity += 2;
            strength += 2;
        }
        else
        {
            Debug.Log("Gain DFC");
            defence += 2;
        }

        currentXP -= xpForNextLevel[playerLevel];
        playerLevel++;
        StartCoroutine(ShowLevelUpSign());
        StartCoroutine(rewardsTexts.ShowLifeRestored());
    }

    public void AddHP(int amountHPToAdd)
    {
        currentHP += amountHPToAdd;
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void EquipMeleeWeapon(ItemsManager meleeWeaponToEquip)
    {
        equipedMeleeWeapon = meleeWeaponToEquip;
        equippedMeleeWeaponName = equipedMeleeWeapon.itemName;
        meleeDamage = equipedMeleeWeapon.weaponStrength;

    }
    
    public void EquipRangeWeapon(ItemsManager rangeWeaponToEquip)
    {
        equipedRangeWeapon = rangeWeaponToEquip;
        equippedRangeWeaponName = equipedRangeWeapon.itemName;
        rangeDamage = equipedRangeWeapon.weaponDexterity;
    }
        
    IEnumerator ShowLevelUpSign()
    {
        levelUp.SetActive(true);
        yield return new WaitForSeconds(2f);
        levelUp.SetActive(false);
    }

}
