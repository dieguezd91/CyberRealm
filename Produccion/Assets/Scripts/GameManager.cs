using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private bool isBattleActive;
    [SerializeField] private bool isTutorial;
    [SerializeField] private bool hasRespawned;
    [SerializeField] private bool isChatting;
    [SerializeField] private bool isInStore;

    [SerializeField] private PlayerStats[] playerStats;

    [SerializeField] private Vector3 lastPosition;

    public GameObject Player => playerObject;
    public bool BattleIsActive { get => isBattleActive; set => isBattleActive = value; }
    public bool Tutorial { get => isTutorial; set => isTutorial = value; }
    public bool Respawned { get => hasRespawned; set => hasRespawned = value; }
    public bool Chatting { get => isChatting; set => isChatting = value; }
    public bool InStore { get => isInStore; set => isInStore = value; }
    public Vector3 LastPosition { get => lastPosition; set => lastPosition = value; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        playerStats = FindObjectsOfType<PlayerStats>();

        playerObject = GameObject.FindGameObjectWithTag("Player");    
    }

    public PlayerStats[] GetPlayerStats()
    {
        return playerStats;
    }

    public void RespawnPlayer()
    {
        lastPosition = new Vector2(-50f, 9f);
        SceneManagerScript.instance.LoadScene("Garage");
        hasRespawned = true;
        PlayerStats.instance.HealFull();
    }
}
