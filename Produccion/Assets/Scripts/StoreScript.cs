using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoreScript : MonoBehaviour
{
    [SerializeField] List<ItemManager> itemInfo;
    [SerializeField] GameObject storeItemTemplate;
    Inventory playerInventory;
    [SerializeField] TextMeshProUGUI credits;

    void Start()
    {
        playerInventory = GameManager.instance.GetComponent<Inventory>();
        var itemTemplate = storeItemTemplate.GetComponent<TemplateStoreItem>();

        foreach (var item in itemInfo)
        {
            itemTemplate.playerInventory = playerInventory;
            itemTemplate.item = item;
            itemTemplate.iconImage.sprite = item.Icon;
            itemTemplate.objectName.text = item.ItemName;
            itemTemplate.priceTag.text = item.ValueCoins.ToString();

            Instantiate(itemTemplate, transform);
        }
    }

    private void Update()
    {
        credits.text = Inventory.instance.Credits.ToString();
    }
}
