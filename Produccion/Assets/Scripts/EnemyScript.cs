
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyType enemyType;
    public string enemyName;
    bool isBossBattle;

    private void Awake()
    {
        if (enemyType == EnemyType.None && !string.IsNullOrEmpty(enemyName))
        {
            enemyType = CombatEnumAdapter.GetEnemyType(enemyName);
        }
    }

    private void Start()
    {
        if (enemyType == EnemyType.JefeMercenario || enemyType == EnemyType.JefeCentral || enemyType == EnemyType.CEOOmniTech)
            isBossBattle = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BattleManager.instance.StartBattle(gameObject, enemyType, false, false, isBossBattle);
        }
    }
}
