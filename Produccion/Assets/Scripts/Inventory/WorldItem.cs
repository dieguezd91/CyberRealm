using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] ItemsManager item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            if (item.itemType == ItemsManager.ItemType.Ammo)
            {
                item.UseItem(0);
            }

            else
                Inventory.instance.AddItems(item);
            //int index = CollectedItemsManager.instance.GetItemNumber(gameObject);
            //CollectedItemsManager.instance.collected[index] = true;
        }
    }
}