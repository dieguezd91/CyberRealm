using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertasDungeon : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        item.SetActive(false);
        door.SetActive(false);
    }
}
