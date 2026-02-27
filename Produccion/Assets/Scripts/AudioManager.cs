using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource lockedUpSfx;
    private float volume;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioClip punchSfx;
    [SerializeField] private AudioClip knifeSfx;
    [SerializeField] private AudioClip batSfx;
    [SerializeField] private AudioClip katanaSfx;
    [SerializeField] private AudioClip pistolSfx;
    [SerializeField] private AudioClip smgSfx;
    [SerializeField] private AudioClip shotgunSfx;

    public AudioSource LockedUpSfx => lockedUpSfx;

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

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void SelectRangeAttackSfx(ItemManager weapon)
    {
        AudioClip clip;
        switch (weapon.WeaponType)
        {
            case WeaponType.Pistola:
                clip = pistolSfx;
                PlaySound(clip);
                break;
            case WeaponType.Subfusil:
                clip = smgSfx;
                PlaySound(clip);
                break;
            case WeaponType.Escopeta:
                clip = shotgunSfx;
                PlaySound(clip);
                break;
            default:
                Debug.Log("Error rangeSFX");
                break;
        }
    }

    public void SelectMeleeAttackSfx(ItemManager weapon)
    {
        AudioClip clip;
        if(weapon == null)
        {
            clip = punchSfx;
            PlaySound(clip);
        }
        else
        {
            switch (weapon.WeaponType)
            {
                case WeaponType.Cuchillo:
                    clip = knifeSfx;
                    PlaySound(clip);
                    break;
                case WeaponType.Bate:
                    clip = batSfx;
                    PlaySound(clip);
                    break;
                case WeaponType.Katana:
                    clip = katanaSfx;
                    PlaySound(clip);
                    break;
                default:
                    Debug.Log("Error meleeSFX");
                    break;
            }
        }
    }
}
