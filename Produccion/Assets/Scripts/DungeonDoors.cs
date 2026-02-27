using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoors : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        item.SetActive(false);
        door.SetActive(false);
    }
}
