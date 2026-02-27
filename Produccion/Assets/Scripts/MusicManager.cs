using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] private AudioSource audioSourceComponent;
    [SerializeField] private AudioClip activeClip;
    [SerializeField] private AudioClip[] songs;
    [SerializeField] private Slider volumeSlider;
    private float volume;

    public AudioSource AudioSource => audioSourceComponent;
    public AudioClip ActiveClip { get => activeClip; set => activeClip = value; }
    public AudioClip[] Songs => songs;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
        audioSourceComponent = GetComponent<AudioSource>();
        audioSourceComponent.volume = volume;
    }

    private void Update()
    {
        if(volumeSlider != null)
        {
            audioSourceComponent.volume = volumeSlider.value;
            volume = volumeSlider.value;
        }
    }
}
