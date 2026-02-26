using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacters : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] public AttackType[] availableAttacks;

    public string characterName;
    public int currentHP, maxHP, dexterity, strength, defence, meleeWeaponDamage, rangeWeaponDamage;
    public bool isDead;
    public int level;

    public ItemsManager equipedRangeWeapon, equipedMeleeWeapon;


    private void Update()
    {
        
    }

    public bool IsPlayer()
    {
        return isPlayer;
    }

    public AttackType[] AttackMovesAvailable()
    {
        return availableAttacks;
    }

    /*public void TakeHPMeleeDamage(int meleeDamageToReceive)
    {
        currentHP -= meleeDamageToReceive;

        if(currentHP < 0)
        {
            currentHP = 0;
        }
    }*/
    
    public void TakeHPDamage(int damageToReceive)
    {
        currentHP -= damageToReceive;

        if(currentHP < 0)
        {
            currentHP = 0;
        }
    }

    public void UseItemInBattle(ItemsManager itemToUse)
    {
        if(itemToUse.itemType == ItemsManager.ItemType.Item)
        {
            if(itemToUse.affectType == ItemsManager.AffectType.HP)
            {
                    AddHP(itemToUse.amountOfAffect);
            }            
        }
        else if(itemToUse.itemType == ItemsManager.ItemType.MeleeWeapon)
        {
            PlayerStats.instance.equipedMeleeWeapon = itemToUse;
            PlayerStats.instance.meleeDamage = itemToUse.weaponStrength;
            PlayerStats.instance.equippedMeleeWeaponName = itemToUse.name;
        }
        else if(itemToUse.itemType == ItemsManager.ItemType.RangeWeapon)
        {
            PlayerStats.instance.equipedRangeWeapon = itemToUse;
            PlayerStats.instance.rangeDamage = itemToUse.weaponDexterity;
            PlayerStats.instance.equippedRangeWeaponName = itemToUse.name;
        }
    }

    public void AddHP(int amountOfAffect)
    {
        currentHP += amountOfAffect;
        if (currentHP > maxHP) currentHP = maxHP;
    }
}