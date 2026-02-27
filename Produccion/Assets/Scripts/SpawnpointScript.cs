using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointScript : MonoBehaviour
{
    public GameObject player;
    public Transform spawnpoint;
    public Transform alternativeSpawn;
    public static SpawnpointScript instance;

    void Start()
    {
        player = GameManager.instance.Player;
        Spawn();
    }

    public void Spawn()
    {
        Debug.Log("spawnMethod");
        switch (SceneManagerScript.instance.CurrentScene)
        {
            case "Bar":
                if (GameManager.instance.Tutorial) player.transform.position = alternativeSpawn.position;
                else  player.transform.position = spawnpoint.position;
                break;
            case "Ciudad":
                player.transform.position = GameManager.instance.LastPosition;
                break;
            case "Garage":
                if (GameManager.instance.Respawned)
                {
                    Debug.Log(GameManager.instance.Respawned);
                    player.transform.position = alternativeSpawn.position;
                    GameManager.instance.Respawned = false;
                }
                else player.transform.position = spawnpoint.position;
                break;
            default:
                player.transform.position = spawnpoint.position;
                break;
        }
    }
}
