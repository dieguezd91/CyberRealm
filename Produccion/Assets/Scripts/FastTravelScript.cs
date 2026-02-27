using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FastTravelScript : MonoBehaviour
{
    [SerializeField] Vector2 garage;
    GameObject player;
    public bool playerNear;
    [SerializeField] TextMeshProUGUI noCredits;

    private void Start()
    {
        player = GameManager.instance.player;
    }

    private void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.Space)) Travel();
    }

    public void Travel()
    {
        if (Inventory.instance.credits >= 50)
        {
            player.transform.position = garage;
            Inventory.instance.credits -= 50;
        }
        else noCredits.text = "Creditos insuficientes";

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(3, 1.25f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerNear = false;
    }
}
