using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    [SerializeField] GameObject objectToActivate;
    [SerializeField] bool active;
    [SerializeField] string questToCheck;
    [SerializeField] bool activateIfComplete;
    [SerializeField] string questToComplete;

    private void Start()
    {
        if (QuestManager.instance.CheckIfComplete(questToCheck))
            gameObject.SetActive(activateIfComplete);
    }
    private void Update()
    {
        active = objectToActivate.activeInHierarchy;
        CheckForCompletion();
    }

    public void CheckForCompletion()
    {
        if (QuestManager.instance.CheckIfComplete(questToCheck))
        {
            objectToActivate.SetActive(activateIfComplete);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && active)
        {
            //activateIfComplete = false;
            QuestManager.instance.MarkQuestComplete(questToComplete);
            gameObject.SetActive(false);
        }
    }
}