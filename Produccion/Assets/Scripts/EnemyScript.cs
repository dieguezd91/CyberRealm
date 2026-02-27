
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public string enemyName;
    bool isBossBattle;

    private void Start()
    {
        if (enemyName == "Jefe Mercenario" || enemyName == "Jefe Central" || enemyName == "CEO de OMNI TECH")
            isBossBattle = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BattleManager.instance.StartBattle(gameObject, enemyName, false, false, isBossBattle);
        }
    }
}
