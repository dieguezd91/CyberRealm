using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour
{
    [SerializeField] private GameObject dialogueManager;
    [SerializeField] private DialogueManager dialogueManagerInstance;
    Inventory inventory;

    public bool beforeFight;
    public bool afterFight;
    public EnemyType enemyType;
    public string enemies;
    bool isBossBattle;
    bool isIsDinnieBattle;
    public bool lockDoor;
    public bool onlyOnTutorial;
    public GameObject door;
    [SerializeField] Collider2D colider;
    [SerializeField] FastTravelScript travelScript;

    private void Awake()
    {
        if (enemyType == EnemyType.None && !string.IsNullOrEmpty(enemies))
        {
            enemyType = CombatEnumAdapter.GetEnemyType(enemies);
        }
    }

    private void Start()
    {
        if (enemyType == EnemyType.JefeMercenario || enemyType == EnemyType.JefeCentral || enemyType == EnemyType.CEOOmniTech)
            isBossBattle = true;
        if(enemyType == EnemyType.Dinnie)
            isIsDinnieBattle = true;
        BattleManager.instance.OnBattleEnd += Activate;
        colider = GetComponent<Collider2D>();
        inventory = GameManager.instance.GetComponent<Inventory>();
        dialogueManagerInstance.OnDialogueEnd += Fight;
        if (onlyOnTutorial && inventory.HasCompletedTutorial)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueManager.SetActive(true);
            if (lockDoor)
                door.SetActive(false);
        }
    }

    private void Fight(object sender,EventArgs e)
    {
        if (beforeFight) BattleManager.instance.StartBattle(gameObject, enemyType, isIsDinnieBattle, false, isBossBattle);
    }

    private void Activate(object sender, EventArgs e)
    {
        if(afterFight && colider != null) colider.enabled = true;
    }
}
