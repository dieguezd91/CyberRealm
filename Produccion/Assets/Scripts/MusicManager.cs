using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioSource audioSource;
    public AudioClip activeClip;
    public AudioClip[] songs;
    public Slider volumeSlider;
    float volume;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
    }

    private void Update()
    {
        if(volumeSlider != null)
        {
            audioSource.volume = volumeSlider.value;
            volume = volumeSlider.value;
        }
    }
}