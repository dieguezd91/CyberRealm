using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    
    [SerializeField] private List<ItemManager> itemsList = new List<ItemManager>();
    [SerializeField] private int credits;
    [FormerlySerializedAs("hasCompletedDinniesTutorial")]
    [SerializeField] private bool hasCompletedTutorial;

    // AMMO
    [SerializeField] private int pistolAmmo;
    [SerializeField] private int shotgunAmmo;
    [SerializeField] private int SMGAmmo;
    [SerializeField] private bool hasAmmo;

    public IReadOnlyList<ItemManager> ItemsList => itemsList;
    public int Credits { get => credits; set => credits = value; }
    public bool HasCompletedTutorial { get => hasCompletedTutorial; set => hasCompletedTutorial = value; }
    
    public int PistolAmmo { get => pistolAmmo; set => pistolAmmo = value; }
    public int ShotgunAmmo { get => shotgunAmmo; set => shotgunAmmo = value; }
    public int SmgAmmo { get => SMGAmmo; set => SMGAmmo = value; }
    public bool HasAmmo { get => hasAmmo; set => hasAmmo = value; }

    private void Start()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(ItemManager item)
    {
        if(item.Type == ItemManager.ItemType.Ammo)
        {
            item.UseItem(0);
        }
        else
        {
            if (item.IsStackable)
            {
                bool itemAlreadyInInventory = false;

                foreach (ItemManager itemInInventory in itemsList)
                {
                    if (itemInInventory.ItemName == item.ItemName)
                    {
                        itemInInventory.Amount++;
                        itemAlreadyInInventory = true;
                    }
                }

                if (!itemAlreadyInInventory)
                {
                    itemsList.Add(item);
                }
            }
            else
            {
                itemsList.Add(item);
            }
        }
        
    }

    public void RemoveItem(ItemManager item)
    {
        if(item.Type == ItemManager.ItemType.Ammo)
        {
            switch(item.WeaponType)
            {
                case WeaponType.Pistola:
                    pistolAmmo--;
                    break;
                case WeaponType.Escopeta:
                    shotgunAmmo--;
                    break;
                case WeaponType.Subfusil:
                    SMGAmmo--;
                    break;
            }
        }
        else
        {
            if (item.IsStackable)
            {
                ItemManager inventoryItem = null;
                foreach (ItemManager itemInInventory in itemsList)
                {
                    if (itemInInventory.ItemName == item.ItemName)
                    {
                        itemInInventory.Amount--;
                        inventoryItem = itemInInventory;
                    }
                }

                if (inventoryItem != null && inventoryItem.Amount <= 0)
                {
                    itemsList.Remove(inventoryItem);
                }
            }
            else
            {
                itemsList.Remove(item);
            }

        }
    }

    public void AddCredits(int amount)
    {
        credits += amount;
        Debug.Log("Ganaste " + amount.ToString() + " creditos");
    }

    public List<ItemManager> GetItemsList()
    {
        return itemsList;
    }
}
