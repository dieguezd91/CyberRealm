using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public enum ItemType { Item, MeleeWeapon, RangeWeapon, Ammo}
    [SerializeField] private ItemType itemType;

    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private int valueCoins;
    [SerializeField] private Sprite icon;

    [SerializeField] private int amountOfAffect;

    public enum AffectType { HP}
    [SerializeField] private AffectType affectType;

    [SerializeField] private int weaponDexterity;
    [SerializeField] private int weaponStrength;

    [SerializeField] private bool isStackable;
    [SerializeField] private int amount;

    public ItemType Type => itemType;
    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public WeaponType WeaponType => weaponType;
    public int ValueCoins => valueCoins;
    public Sprite Icon => icon;
    public int AmountOfAffect => amountOfAffect;
    public AffectType Affect => affectType;
    public int WeaponDexterity => weaponDexterity;
    public int WeaponStrength => weaponStrength;
    public bool IsStackable => isStackable;
    public int Amount { get => amount; set => amount = value; }

    private void Awake()
    {
        if (weaponType == WeaponType.None && !string.IsNullOrEmpty(itemName))
        {
            weaponType = CombatEnumAdapter.GetWeaponType(itemName);
        }
    }

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
            switch (weaponType)
            {
                case WeaponType.Pistola:
                    Inventory.instance.PistolAmmo += 7;
                    break;
                case WeaponType.Escopeta:
                    Inventory.instance.ShotgunAmmo += 2;
                    break;
                case WeaponType.Subfusil:
                    Inventory.instance.SmgAmmo += 10;
                    break;
            }
        }
        else if (affectType == AffectType.HP)
        {            
            selectedCharacter.AddHealth(amountOfAffect);
        }
        
        if(itemType == ItemType.MeleeWeapon)
        {
            if (selectedCharacter.EquippedMeleeWeaponName != "")
            {
                Inventory.instance.AddItem(selectedCharacter.EquippedMeleeWeapon);
            }

            selectedCharacter.EquipMeleeWeapon(this);
        }
        
        if(itemType == ItemType.RangeWeapon)
        {
            if(selectedCharacter.EquippedRangeWeaponName != "")
            {
                Inventory.instance.AddItem(selectedCharacter.EquippedRangeWeapon);
            }

            selectedCharacter.EquipRangeWeapon(this);
        }
    }

    public void SelfDestroy()
    {
        gameObject.SetActive(false);
    }
}
