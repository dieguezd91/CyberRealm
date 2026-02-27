using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<ItemsManager> itemsList;
    public int credits;
    public bool hasCompletedDinniesTutorial;

    //AMMO
    public int pistolAmmo;
    public int shotgunAmmo;
    public int SMGAmmo;
    public bool hasAmmo;


    private void Start()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);

        itemsList = new List<ItemsManager>();
    }

    public void AddItems(ItemsManager item)
    {
        if(item.itemType == ItemsManager.ItemType.Ammo)
        {
            item.UseItem(0);
        }
        else
        {
            if (item.isStackable)
            {
                bool itemAlreadyInInventory = false;

                foreach (ItemsManager itemInInventory in itemsList)
                {
                    if (itemInInventory.itemName == item.itemName)
                    {
                        itemInInventory.amount++;
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

    public void RemoveItem(ItemsManager item)
    {
        if(item.itemType == ItemsManager.ItemType.Ammo)
        {
            switch(item.itemName)
            {
                case "Balas de pistola":
                    pistolAmmo--;
                    break;
                case "Cartuchos de escopeta":
                    shotgunAmmo--;
                    break;
                case "Balas de subfusil":
                    SMGAmmo--;
                    break;
            }
        }
        else
        {
            if (item.isStackable)
            {
                ItemsManager inventoryItem = null;
                foreach (ItemsManager itemInInventory in itemsList)
                {
                    if (itemInInventory.itemName == item.itemName)
                    {
                        itemInInventory.amount--;
                        inventoryItem = itemInInventory;
                    }
                }

                if (inventoryItem != null && inventoryItem.amount <= 0)
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

    public void AddCredits(int creditsToGive)
    {
        credits += creditsToGive;
        Debug.Log("Ganaste " + creditsToGive.ToString() + " creditos");
    }

    public List<ItemsManager> GetItemsList()
    {
        return itemsList;
    }
}
