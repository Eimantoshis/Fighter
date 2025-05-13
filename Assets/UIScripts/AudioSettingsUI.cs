using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour {
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button backButton;

    [SerializeField] private GameObject mainMenu;

    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    // Start is called before the first frame update
    void Start() {
        sfxSlider.value = AudioManager.Instance.sfxVolume;
        musicSlider.value = AudioManager.Instance.musicVolume;
        
        
        sfxVolumeText.text = ((int)(AudioManager.Instance.sfxVolume * 100)).ToString();
        musicVolumeText.text = ((int)(AudioManager.Instance.musicVolume * 100)).ToString();
        
        sfxSlider.onValueChanged.AddListener(val => {
            AudioManager.Instance.SetSFXVolume(val);
            sfxVolumeText.text = ((int)(val * 100)).ToString();
        });
        musicSlider.onValueChanged.AddListener(val => {
            AudioManager.Instance.SetMusicVolume(val);
            musicVolumeText.text = ((int)(val * 100)).ToString();
        });
        
        
        backButton.onClick.AddListener(OnBackButtonClicked);
        Hide();
    }

    private void OnBackButtonClicked() {
        mainMenu.gameObject.GetComponent<MenuUI>().BackToMainMenu();
        Hide();

    }

    private void Hide() {
        gameObject.SetActive(false);
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
