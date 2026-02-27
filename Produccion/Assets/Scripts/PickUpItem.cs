using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] GameObject item;
    [SerializeField] ItemsManager invItem;
    [SerializeField] bool openDoorAfterFight;
    [SerializeField] GameObject door;
    [SerializeField] GameObject objectToDisable;
    [SerializeField] Collider2D eventToEnable;
    Inventory inventory;
    Collider2D collider;
    [SerializeField] bool addToInv;

    private void Start()
    {
        inventory = GameManager.instance.GetComponent<Inventory>();
        collider = gameObject.GetComponent<Collider2D>();
        if(inventory.hasCompletedDinniesTutorial)
        {
            item.SetActive(false);
            collider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        item.SetActive(false);
        collider.enabled = false;
        Inventory.instance.itemsList.Add(invItem);
        GameManager.instance.tutorial = false;
        eventToEnable.enabled = true;
        door.SetActive(true);
        objectToDisable.SetActive(false);
        Inventory.instance.hasCompletedDinniesTutorial = true;
    }

    void OpenDoor(object sender, EventArgs e)
    {
        door.SetActive(true);
        objectToDisable.SetActive(false);
        Inventory.instance.hasCompletedDinniesTutorial = true;
    }
}
