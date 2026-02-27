using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;
    
    [FormerlySerializedAs("scene")]
    [SerializeField] private string currentScene;

    [SerializeField] private Vector2 spawnpoint;

    public string CurrentScene { get => currentScene; set => currentScene = value; }
    public Vector2 Spawnpoint => spawnpoint;

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

        if (string.IsNullOrEmpty(currentScene))
        {
            currentScene = SceneManager.GetActiveScene().name;
        }
    }

    public void LoadScene(string newScene)
    {
        Debug.Log(newScene + " loaded");
        MusicManager.instance.AudioSource.Stop();
        currentScene = newScene;
        CheckActiveClip();
        MusicManager.instance.AudioSource.clip = MusicManager.instance.ActiveClip;
        MusicManager.instance.AudioSource.Play();
        if (newScene == "Fabrica" || newScene == "Central de seguridad" || newScene == "Omni-Tech")
            AudioManager.instance.LockedUpSfx.enabled = true;
        else AudioManager.instance.LockedUpSfx.enabled = false;
        if (newScene == "MainMenu")
        {
            Destroy(GameManager.instance.Player);
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
        switch (currentScene)
        {
            case "MainMenu":
                MusicManager.instance.ActiveClip = MusicManager.instance.Songs[6];
                break;
            case "Bar":
                MusicManager.instance.ActiveClip = MusicManager.instance.Songs[1];
                break;
            case "Garage":
                MusicManager.instance.ActiveClip = MusicManager.instance.Songs[5];
                break;
            case "Ciudad":
                MusicManager.instance.ActiveClip = MusicManager.instance.Songs[0];
                break;
            case "Store":
                MusicManager.instance.ActiveClip = MusicManager.instance.Songs[0];
                break;
            case "Fabrica":
                MusicManager.instance.ActiveClip = MusicManager.instance.Songs[2];
                break;
            case "Central de seguridad":
                MusicManager.instance.ActiveClip = MusicManager.instance.Songs[4];
                break;
            case "Omni-Tech":
                MusicManager.instance.ActiveClip = MusicManager.instance.Songs[3];
                break;
            default:
                break;
        }
    }
}
