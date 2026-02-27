using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameDataController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerStats playerStats;

    private string saveArchives;
    [SerializeField] private GameData gameData = new GameData();

    public GameData Data => gameData;

    private void Awake()
    {
        saveArchives = Application.dataPath + "/datos.json";
    }

    private void Start()
    {
        player = GameManager.instance.Player;
        playerStats = GameManager.instance.Player.GetComponent<PlayerStats>();
    }

    public void LoadData()
    {
        if (File.Exists(saveArchives))
        {
            string content = File.ReadAllText(saveArchives);
            gameData = JsonUtility.FromJson<GameData>(content);

            SceneManagerScript.instance.LoadScene(gameData.Scene);
            Inventory.instance.Credits = gameData.Credits;
            playerStats.SetStats(gameData.Level, gameData.Xp, gameData.LifePoints, gameData.Strength, gameData.Dexterity, gameData.Defence);
            QuestManager.instance.CompletedQuests = gameData.CompletedQuests;
        }
        else
        {
            Debug.Log("El archivo no existe");
        }
    }

    public void SaveData()
    {
        GameData newData = new GameData()
        {
            Position = player.transform.position,
            LifePoints = playerStats.CurrentHealth,
            Xp = playerStats.CurrentXp,
            Dexterity = playerStats.Dexterity,
            Strength = playerStats.Strength,
            Defence = playerStats.Defence,
            Level = playerStats.PlayerLevel,
            Credits = Inventory.instance.Credits,
            Scene = SceneManagerScript.instance.CurrentScene,
            CompletedQuests = QuestManager.instance.CompletedQuests,
        };

        string JSONchain = JsonUtility.ToJson(newData);
        File.WriteAllText(saveArchives, JSONchain);

        Debug.Log("Data saved");
    }
}
