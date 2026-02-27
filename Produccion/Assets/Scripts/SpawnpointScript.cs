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
        player = GameManager.instance.player;
        Spawn();
    }

    public void Spawn()
    {
        Debug.Log("spawnMethod");
        switch (SceneManagerScript.instance.scene)
        {
            case "Bar":
                if (GameManager.instance.tutorial) player.transform.position = alternativeSpawn.position;
                else  player.transform.position = spawnpoint.position;
                break;
            case "Ciudad":
                player.transform.position = GameManager.instance.lastPosition;
                break;
            case "Garage":
                if (GameManager.instance.respawned)
                {
                    Debug.Log(GameManager.instance.respawned);
                    player.transform.position = alternativeSpawn.position;
                    GameManager.instance.respawned = false;
                }
                else player.transform.position = spawnpoint.position;
                break;
            default:
                player.transform.position = spawnpoint.position;
                break;
        }
    }
}
