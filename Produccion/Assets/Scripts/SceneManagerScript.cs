using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;
    public string scene;

    public Vector2 spawnpoint;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void LoadScene(string newScene)
    {
        Debug.Log(newScene + " loaded");
        MusicManager.instance.audioSource.Stop();
        scene = newScene;
        CheckActiveClip();
        MusicManager.instance.audioSource.clip = MusicManager.instance.activeClip;
        MusicManager.instance.audioSource.Play();
        if (newScene == "Fabrica" || newScene == "Central de seguridad" || newScene == "Omni-Tech")
            AudioManager.instance.lockedUpSFX.enabled = true;
        else AudioManager.instance.lockedUpSFX.enabled = false;
        if (newScene == "MainMenu")
        {
            Destroy(GameManager.instance.player);
            Destroy(GameManager.instance.gameObject);
            Destroy(QuestManager.instance.gameObject);
            Destroy(BattleManager.instance.gameObject);
            Destroy(MenuManager.instance.gameObject);
            Destroy(MusicManager.instance.gameObject);
            Destroy(AudioManager.instance.gameObject);
        }
        SceneManager.LoadScene(newScene);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("I'm outta here!");
    }
    void CheckActiveClip()
    {
        switch (scene)
        {
            case "MainMenu":
                MusicManager.instance.activeClip = MusicManager.instance.songs[6];
                break;
            case "Bar":
                MusicManager.instance.activeClip = MusicManager.instance.songs[1];
                break;
            case "Garage":
                MusicManager.instance.activeClip = MusicManager.instance.songs[5];
                break;
            case "Ciudad":
                MusicManager.instance.activeClip = MusicManager.instance.songs[0];
                break;
            case "Store":
                MusicManager.instance.activeClip = MusicManager.instance.songs[0];
                break;
            case "Fabrica":
                MusicManager.instance.activeClip = MusicManager.instance.songs[2];
                break;
            case "Central de seguridad":
                MusicManager.instance.activeClip = MusicManager.instance.songs[4];
                break;
            case "Omni-Tech":
                MusicManager.instance.activeClip = MusicManager.instance.songs[3];
                break;
            default:
                break;
        }
    }
}
