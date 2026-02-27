using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    bool pjNearby;
    public GameObject menu;
    PlayerController playerController;
    float lastSpeed;

    private void Start()
    {
        playerController = GameManager.instance.player.GetComponent<PlayerController>();
    }
    void Update()
    {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 1.5f), 1f).CompareTag("Player")) pjNearby = true;
        else pjNearby = false;

        if (pjNearby && Input.GetKeyDown(KeyCode.Space) && !menu.activeInHierarchy)
        {
            if(MenuManager.instance.menu.activeInHierarchy)
                MenuManager.instance.OpenCloseInventory();
            lastSpeed = playerController.moveSpeed;
            playerController.moveSpeed = 0;
            menu.SetActive(true);
            GameManager.instance.inStore = true;

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(false);
            GameManager.instance.inStore = false;
            playerController.moveSpeed = lastSpeed;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector2(transform.position.x, transform.position.y - 1.5f), 1f);
    }
}
