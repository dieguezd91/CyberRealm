using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TemplateStoreItem : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI priceTag;
    public TextMeshProUGUI objectName;
    public Button buyButton;
    int price;
    public ItemManager item;
    public Inventory playerInventory;


    void Start()
    {
        playerInventory = Inventory.instance.GetComponent<Inventory>();
        price = int.Parse(priceTag.text);
    }

    void Update()
    {
        if (price > playerInventory.Credits)
        {
            buyButton.interactable = false;
        }
    }

    public void BuyItem()
    {
        playerInventory.Credits -= price;
        playerInventory.AddItem(item);
    }

    public void SellItem()
    {
        if(item.Type != ItemManager.ItemType.Ammo)
        {
            if (playerInventory.GetItemsList().Contains(item))
            {
                playerInventory.Credits += price;
                playerInventory.RemoveItem(item);
            }
            else Debug.Log("No posees este item");
        }
        else
        {
            switch (item.ItemName)
            {
                case "Balas de pistola":
                    if (Inventory.instance.PistolAmmo >= 7)
                    {
                        Inventory.instance.PistolAmmo -= 7;
                        playerInventory.Credits += price;
                    }
                    else Debug.Log("No posees este item");
                    break;
                case "Cartuchos de escopeta":
                    if (Inventory.instance.ShotgunAmmo >= 2)
                    {
                        Inventory.instance.ShotgunAmmo -= 2;
                        playerInventory.Credits += price;
                    }
                    else Debug.Log("No posees este item");
                    break;
                case "Balas de subfusil":
                    if (Inventory.instance.SmgAmmo >= 10)
                    {
                        Inventory.instance.SmgAmmo -= 10;
                        playerInventory.Credits += price;
                    }
                    else Debug.Log("No posees este item");
                    break;
            }
        }
    }
}
