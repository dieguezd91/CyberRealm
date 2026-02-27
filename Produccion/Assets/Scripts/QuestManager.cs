using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    [SerializeField] string[] questNames;
    [SerializeField] string[] questDescriptions;
    public bool[] questCompleted;

    public static QuestManager instance;

    [SerializeField] GameObject MissionCompletedUI;
    [SerializeField] TextMeshProUGUI newObjective;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        questCompleted = new bool[questNames.Length];
    }

    public int GetQuestNumber(string questToFind)
    {
        for (int i = 0; i < questNames.Length; i++)
        {
            if (questNames[i] == questToFind)
            {
                return i;
            }
        }

        Debug.LogWarning("Quest: " + questToFind + " does not exist");
        return 0;
    }

    public bool CheckIfComplete(string questToCheck)
    {
        int questNumberToCheck = GetQuestNumber(questToCheck);

        return questCompleted[questNumberToCheck];
    }

    //public void UpdateQuestObjects()
    //{
    //    QuestObject[] questObjects = FindObjectsOfType<QuestObject>();

    //    if(questObjects.Length > 0)
    //    {
    //        foreach(QuestObject questObject in questObjects)
    //        {
    //            questObject.CheckForCompletion();
    //        }
    //    }
    //}

    public void MarkQuestComplete(string questToMark)
    {
        int questNumberToCheck = GetQuestNumber(questToMark);
        questCompleted[questNumberToCheck] = true;

        if(questToMark != "a")
            StartCoroutine(ShowUI());
        //OnQuestMarked?.Invoke(this, EventArgs.Empty);
        //UpdateQuestObjects();
    }

    IEnumerator ShowUI()
    {
        MissionCompletedUI.SetActive(true);
        int currentMission = GetObjectiveDescription();
        newObjective.text = questDescriptions[currentMission];
        yield return new WaitForSeconds(6.5f);
        MissionCompletedUI.SetActive(false);
    }

    int GetObjectiveDescription()
    {
        for(int i = 0; i < questCompleted.Length; i++)
        {
            if (!questCompleted[i]) return i;
        }

        return 0;
    }
}
