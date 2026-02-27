using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderInLayerScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (GameManager.instance.player.transform.position.y < transform.position.y) spriteRenderer.sortingOrder = -1;
        else spriteRenderer.sortingOrder = 1;
    }
}
