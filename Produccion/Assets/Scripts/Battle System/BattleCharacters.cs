using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacters : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    [SerializeField] private AttackType[] availableAttacks;

    [SerializeField] private string characterName;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int dexterity;
    [SerializeField] private int strength;
    [SerializeField] private int defence;
    [SerializeField] private int meleeWeaponDamage;
    [SerializeField] private int rangeWeaponDamage;
    [SerializeField] private bool isDead;
    [SerializeField] private int level;

    [SerializeField] private ItemManager equippedRangeWeapon;
    [SerializeField] private ItemManager equippedMeleeWeapon;

    public bool IsPlayer => isPlayer;
    public AttackType[] AvailableAttacks => availableAttacks;
    public string CharacterName => characterName;
    public EnemyType EnemyType => enemyType;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int Dexterity => dexterity;
    public int Strength => strength;
    public int Defence => defence;
    public int MeleeWeaponDamage => meleeWeaponDamage;
    public int RangeWeaponDamage => rangeWeaponDamage;
    public bool IsDead => isDead;
    public int Level => level;
    public ItemManager EquippedRangeWeapon => equippedRangeWeapon;
    public ItemManager EquippedMeleeWeapon => equippedMeleeWeapon;

    private void Awake()
    {
        if (enemyType == EnemyType.None && !string.IsNullOrEmpty(characterName) && !isPlayer)
        {
            enemyType = CombatEnumAdapter.GetEnemyType(characterName);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        if (currentHealth == 0) isDead = true;
    }

    public void SetStats(int hp, int maxHp, int lvl, int dex, int str, int def, int meleeDmg, int rangeDmg)
    {
        currentHealth = hp;
        maxHealth = maxHp;
        level = lvl;
        dexterity = dex;
        strength = str;
        defence = def;
        meleeWeaponDamage = meleeDmg;
        rangeWeaponDamage = rangeDmg;
    }

    public void SetEquippedWeapons(ItemManager melee, ItemManager range)
    {
        equippedMeleeWeapon = melee;
        equippedRangeWeapon = range;
    }

    public void UseItemInBattle(ItemManager itemToUse)
    {
        if(itemToUse.Type == ItemManager.ItemType.Item)
        {
            if(itemToUse.Affect == ItemManager.AffectType.HP)
            {
                    AddHealth(itemToUse.AmountOfAffect);
            }            
        }
        else if(itemToUse.Type == ItemManager.ItemType.MeleeWeapon)
        {
            PlayerStats.instance.EquipMeleeWeapon(itemToUse);
        }
        else if(itemToUse.Type == ItemManager.ItemType.RangeWeapon)
        {
            PlayerStats.instance.EquipRangeWeapon(itemToUse);
        }
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
}
