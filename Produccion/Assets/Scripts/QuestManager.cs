using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private string[] questNames;
    [SerializeField] private string[] questDescriptions;
    
    [FormerlySerializedAs("questCompleted")]
    [SerializeField] private bool[] completedQuests;

    public static QuestManager instance;

    [SerializeField] private GameObject missionCompletedUi;
    [SerializeField] private TextMeshProUGUI newObjective;

    public bool[] CompletedQuests { get => completedQuests; set => completedQuests = value; }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        completedQuests = new bool[questNames.Length];
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

        return completedQuests[questNumberToCheck];
    }

    public void MarkQuestComplete(string questToMark)
    {
        int questNumberToCheck = GetQuestNumber(questToMark);
        completedQuests[questNumberToCheck] = true;

        if(questToMark != "a")
            StartCoroutine(ShowUI());
    }

    IEnumerator ShowUI()
    {
        missionCompletedUi.SetActive(true);
        int currentMission = GetObjectiveDescription();
        newObjective.text = questDescriptions[currentMission];
        yield return new WaitForSeconds(6.5f);
        missionCompletedUi.SetActive(false);
    }

    int GetObjectiveDescription()
    {
        for(int i = 0; i < completedQuests.Length; i++)
        {
            if (!completedQuests[i]) return i;
        }

        return 0;
    }
}
