using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomBattle : MonoBehaviour
{
    Collider2D colider;
    private void Start()
    {
        colider = GetComponent<Collider2D>();
        //BattleManager.instance.OnBattleEnd += Scape;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            int i = UnityEngine.Random.Range(1, 101);
            string enemy;
            if (i <= 5) enemy = "Vagabundo";
            else enemy = "Traficante";

            if ( i <= 10)
            {
                Debug.Log("Combate random!");
                BattleManager.instance.StartBattle(gameObject, enemy, false, true, false);
            }
        }
    }

    void Scape( object sender, EventArgs e)
    {
        StartCoroutine(Inmunity());
    }

    public IEnumerator Inmunity()
    {
        Debug.Log("aaaaa");
        colider.enabled = false;
        yield return new WaitForSeconds(5f);
        colider.enabled = true;
    }
}