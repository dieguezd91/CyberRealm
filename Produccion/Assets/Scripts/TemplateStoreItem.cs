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
    public ItemsManager item;
    public Inventory playerInventory;


    void Start()
    {
        playerInventory = Inventory.instance.GetComponent<Inventory>();
        price = int.Parse(priceTag.text);
    }

    void Update()
    {
        if (price > playerInventory.credits)
        {
            buyButton.interactable = false;
        }
    }

    public void BuyItem()
    {
        playerInventory.credits -= price;
        playerInventory.AddItems(item);
    }

    public void SellItem()
    {
        if(item.itemType != ItemsManager.ItemType.Ammo)
        {
            if (playerInventory.itemsList.Contains(item))
            {
                playerInventory.credits += price;
                playerInventory.RemoveItem(item);
            }
            else Debug.Log("No posees este item");
        }
        else
        {
            switch (item.itemName)
            {
                case "Balas de pistola":
                    if (Inventory.instance.pistolAmmo >= 7)
                    {
                        Inventory.instance.pistolAmmo -= 7;
                        playerInventory.credits += price;
                    }
                    else Debug.Log("No posees este item");
                    break;
                case "Cartuchos de escopeta":
                    if (Inventory.instance.shotgunAmmo >= 2)
                    {
                        Inventory.instance.shotgunAmmo -= 2;
                        playerInventory.credits += price;
                    }
                    else Debug.Log("No posees este item");
                    break;
                case "Balas de subfusil":
                    if (Inventory.instance.SMGAmmo >= 10)
                    {
                        Inventory.instance.SMGAmmo -= 10;
                        playerInventory.credits += price;
                    }
                    else Debug.Log("No posees este item");
                    break;
            }
        }
    }
}
