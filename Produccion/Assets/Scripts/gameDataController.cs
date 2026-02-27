using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class gameDataController : MonoBehaviour
{
    public GameObject player;
    public PlayerStats playerStats;

    private string saveArchives;
    public gameData gameData = new gameData();

    private void Awake()
    {
        saveArchives = Application.dataPath + "/datos.json";
    }

    private void Start()
    {
        player = GameManager.instance.player;
        playerStats = GameManager.instance.player.GetComponent<PlayerStats>();
    }
    public void LoadData()
    {
        if (File.Exists(saveArchives))
        {
            string content = File.ReadAllText(saveArchives);
            gameData = JsonUtility.FromJson<gameData>(content);

            SceneManagerScript.instance.LoadScene(gameData.scene);
            Inventory.instance.credits = gameData.credits;
            playerStats.currentXP = gameData.xp;
            playerStats.playerLevel = gameData.level;
            playerStats.currentHP = gameData.lifePoints;
            playerStats.strength = gameData.strength;
            playerStats.dexterity = gameData.dexterity;
            playerStats.defence = gameData.defence;
            QuestManager.instance.questCompleted = gameData.completedQuests;
        }
        else
        {
            Debug.Log("El archivo no existe");
        }
    }

    public void SaveData()
    {
        gameData newData = new gameData()
        {
            position = player.transform.position,
            lifePoints = playerStats.currentHP,
            xp = playerStats.currentXP,
            dexterity = playerStats.dexterity,
            strength = playerStats.strength,
            defence = playerStats.defence,
            level = playerStats.playerLevel,
            credits = Inventory.instance.credits,
            scene = SceneManagerScript.instance.scene,
            completedQuests = QuestManager.instance.questCompleted,
        };

        string JSONchain = JsonUtility.ToJson(newData);
        File.WriteAllText(saveArchives, JSONchain);

        Debug.Log("Data saved");
    }
}
