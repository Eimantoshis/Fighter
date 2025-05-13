using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    
    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Audio Clips")]
    public AudioClip clickSound;
    public AudioClip explodeSound;
    public AudioClip playerHitSound;
    public AudioClip playerDeathSound;
    public AudioClip shootSound;
    public AudioClip dashSound;

    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    [Range(0f, 1f)]
    public float musicVolume = 1f;

    public void PlayClick() => PlaySFX(clickSound);
    public void PlayExplode() => PlaySFX(explodeSound);
    public void PlayPlayerHit() => PlaySFX(playerHitSound);
    public void PlayPlayerDeath() => PlaySFX(playerDeathSound);
    public void PlayShootSound() => PlaySFX(shootSound);
    public void PlayDashSound() => PlaySFX(dashSound);


    public void PlaySFX(AudioClip clip) {
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetSFXVolume(float volume) {
        sfxVolume = volume;
        SaveVolumes();
        UpdateVolumes();
    }

    public void SetMusicVolume(float volume) {
        musicVolume = volume;
        SaveVolumes();
        UpdateVolumes();
    }

    private void SaveVolumes() {
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    private void LoadVolumes() {
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", sfxVolume);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", musicVolume);
        UpdateVolumes();
    }

    private void UpdateVolumes() {
        sfxSource.volume = sfxVolume;
        musicSource.volume = musicVolume;
    }
    
    

    private void Awake() {
        Instance = this;
        LoadVolumes();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
