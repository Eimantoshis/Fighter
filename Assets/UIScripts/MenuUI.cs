using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Button playButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject audioSettingsPanel;
    [SerializeField] private GameObject controlChangePanel;
    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        audioButton.onClick.AddListener(OnAudioClicked);
        controlsButton.onClick.AddListener(OnControlsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        Hide();
    }

    // Update is called once per frame
    void Update() {
        OpenClose();
    }

    private void OpenClose() {
        if (InputManager.Instance.isRebinding) return; // so I don't get stuck in a loop of rebinding
        
        if (Input.GetKeyDown(InputManager.Instance.Pause)) {
            if (isOpen) {
                Hide();
                Time.timeScale = 1f;
            } else {
                Show();
                Time.timeScale = 0f;
            }
            isOpen = !isOpen;
        }
    }


    private void Hide() {
        menu.SetActive(false);
    }

    private void Show() {
        menu.SetActive(true);
    }

    private void HideButtons() {
        playButton.gameObject.SetActive(false);
        audioButton.gameObject.SetActive(false);
        controlsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        
    }

    public void BackToMainMenu() {
        playButton.gameObject.SetActive(true);
        audioButton.gameObject.SetActive(true);
        controlsButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        title.text = "MENU";
    }
    

    private void OnPlayClicked() {
        Hide();
        isOpen = false;
        Time.timeScale = 1f;
    }

    private void OnAudioClicked() {
        HideButtons();
        audioSettingsPanel.SetActive(true);
        title.text = "AUDIO";
    }

    private void OnControlsClicked() {
        HideButtons();
        controlChangePanel.SetActive(true);
        title.text = "CONTROLS";
    }

    private void OnQuitClicked() {
        Debug.Log("Quit clicked");
        Application.Quit();
    }
    
}
