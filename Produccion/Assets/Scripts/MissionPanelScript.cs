using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanelScript : MonoBehaviour
{
    [SerializeField] Image[] ticks;

    void Update()
    {
        for(int i = 0; i < QuestManager.instance.questCompleted.Length - 1; i++)
        {
            Debug.Log(i);
            if (QuestManager.instance.questCompleted[i+1])
            {
                Debug.Log(i);
                ticks[i].color = Color.green;
            }
            else ticks[i].color = Color.red;
        }
    }
}
