using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager instance;

    public enum ItemType { Item, MeleeWeapon, RangeWeapon, Ammo}
    public ItemType itemType;

    public string itemName, itemDescription;
    public int valueCoins;
    public Sprite icon;

    public int amountOfAffect;

    public enum AffectType { HP}
    public AffectType affectType;

    public int weaponDexterity;
    public int weaponStrength;

    public bool isStackable;
    public int amount;

    private void Start()
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
    }

    public void UseItem(int characterToUseOn)
    {
        PlayerStats selectedCharacter = GameManager.instance.GetPlayerStats()[characterToUseOn];
        
        if (itemType == ItemType.Ammo)
        {
            Debug.Log("Obtener munición");
            Debug.Log(itemName);
            switch (itemName)
            {
                case "Balas de pistola":
                    Inventory.instance.pistolAmmo += 7;
                    Debug.Log("Municion de pistola obtenida");
                    break;
                case "Cartuchos de escopeta":
                    Inventory.instance.shotgunAmmo += 2;
                    break;
                case "Balas de subfusil":
                    Inventory.instance.SMGAmmo +=10;
                    break;
            }
        }
        else if (affectType == AffectType.HP)
        {            
            selectedCharacter.AddHP(amountOfAffect);
            if (selectedCharacter.currentHP > selectedCharacter.maxHP)
                selectedCharacter.currentHP = selectedCharacter.maxHP;
        }
        if(itemType == ItemType.MeleeWeapon)
        {
            if (selectedCharacter.equippedMeleeWeaponName != "")
            {
                Inventory.instance.AddItems(selectedCharacter.equipedMeleeWeapon);
            }

            selectedCharacter.EquipMeleeWeapon(this);
        }
        if(itemType == ItemType.RangeWeapon)
        {
            if(selectedCharacter.equippedRangeWeaponName != "")
            {
                Inventory.instance.AddItems(selectedCharacter.equipedRangeWeapon);
            }

            selectedCharacter.EquipRangeWeapon(this);
        }
    }

    public void SelfDestroy()
    {
        gameObject.SetActive(false);
    }
}
