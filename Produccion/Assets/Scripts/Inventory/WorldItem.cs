using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] ItemManager item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            if (item.Type == ItemManager.ItemType.Ammo)
            {
                item.UseItem(0);
            }

            else
                Inventory.instance.AddItem(item);
            //int index = CollectedItemManager.instance.GetItemNumber(gameObject);
            //CollectedItemManager.instance.collected[index] = true;
        }
    }
}
