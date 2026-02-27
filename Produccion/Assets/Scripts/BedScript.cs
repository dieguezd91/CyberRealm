using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedScript : MonoBehaviour
{
    PlayerStats player;
    [SerializeField] GameObject HPRestoredSign;
    public bool pjNearBy;

    private void Start()
    {
        player = GameManager.instance.player.GetComponent<PlayerStats>();
        HPRestoredSign = MenuManager.instance.rewardsTexts.lifeRestoredText;
    }

    private void Update()
    {
        if(pjNearBy && Input.GetKeyDown(KeyCode.Space))
        {
            player.currentHP = player.maxHP;
            StartCoroutine(ShowSign());
        }
    }

    private IEnumerator ShowSign()
    {
        HPRestoredSign.SetActive(true);
        yield return new WaitForSeconds(2f);
        HPRestoredSign.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) pjNearBy = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) pjNearBy = false;
    }
}
