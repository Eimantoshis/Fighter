using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlChangeUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject controlChangePanel;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject mainMenu;

    [Header("Keybind Buttons")]
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button dashButton;
    [SerializeField] private Button cameraLeftButton;
    [SerializeField] private Button cameraRightButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button autoFireButton;
    
    [Header("Keybind BUTTON Text")]
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI dashText;
    [SerializeField] private TextMeshProUGUI cameraLeftText;
    [SerializeField] private TextMeshProUGUI cameraRightText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI autoFireText;
    
    [Header("Listening Panel")]
    [SerializeField] private GameObject listeningPanel;
    [SerializeField] private TextMeshProUGUI listeningActionText;

    private string currentActionToRebind = "";
    private bool isListeningForInput = false;

    private void Start() {
        UpdateAllKeybindTexts();
        
        moveUpButton.onClick.AddListener(() => StartRebinding(("MoveUp")));
        moveDownButton.onClick.AddListener(() => StartRebinding(("MoveDown")));
        moveLeftButton.onClick.AddListener(() => StartRebinding(("MoveLeft")));
        moveRightButton.onClick.AddListener(() => StartRebinding(("MoveRight")));
        dashButton.onClick.AddListener(() => StartRebinding(("Dash")));
        cameraLeftButton.onClick.AddListener(() => StartRebinding(("CameraLeft")));
        cameraRightButton.onClick.AddListener(() => StartRebinding(("CameraRight")));
        pauseButton.onClick.AddListener(() => StartRebinding(("Pause")));
        autoFireButton.onClick.AddListener(() => StartRebinding(("AutoFire")));
        
        backButton.onClick.AddListener(OnBackButtonClicked);
        
        listeningPanel.SetActive(false);
        Hide();
    }


    private void Update() {
        if (isListeningForInput) {
            if (Input.anyKeyDown) {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode))) {
                    if (Input.GetKeyDown(keyCode)) {
                        if (keyCode >= KeyCode.Mouse0 && keyCode <= KeyCode.Mouse6) {
                            continue; // skip mouse inputs
                        }

                        
                        InputManager.Instance.SetKey(currentActionToRebind, keyCode);
                        
                        UpdateKeybindText(currentActionToRebind);
                        StopRebinding();
                        break;
                    }
                }
            }
        }
    }

    private void StartRebinding(string actionName) {
        InputManager.Instance.isRebinding = true;
        currentActionToRebind = actionName;
        isListeningForInput = true;
        listeningPanel.SetActive(true);
        listeningActionText.text = "Press any key to rebind " + actionName;
    }

    private void StopRebinding() {
        InputManager.Instance.isRebinding = false;
        currentActionToRebind = "";
        isListeningForInput = false;
        listeningPanel.SetActive(false);
    }

    private void UpdateAllKeybindTexts() {
        moveUpText.text = InputManager.Instance.MoveUp.ToString();
        moveDownText.text = InputManager.Instance.MoveDown.ToString();
        moveLeftText.text = InputManager.Instance.MoveLeft.ToString();
        moveRightText.text = InputManager.Instance.MoveRight.ToString();
        dashText.text = InputManager.Instance.Dash.ToString();
        cameraLeftText.text = InputManager.Instance.CameraLeft.ToString();
        cameraRightText.text = InputManager.Instance.CameraRight.ToString();
        pauseText.text = InputManager.Instance.Pause.ToString();
        autoFireText.text = InputManager.Instance.AutoFire.ToString();
    }

    private void UpdateKeybindText(string actionName) {
        switch (actionName) {
            case "MoveUp":
                moveUpText.text = InputManager.Instance.MoveUp.ToString();
                break;
            case "MoveDown":
                moveDownText.text = InputManager.Instance.MoveDown.ToString();
                break;
            case "MoveLeft":
                moveLeftText.text = InputManager.Instance.MoveLeft.ToString();
                break;
            case "MoveRight":
                moveRightText.text = InputManager.Instance.MoveRight.ToString();
                break;
            case "Dash":
                dashText.text = InputManager.Instance.Dash.ToString();
                break;
            case "CameraLeft":
                cameraLeftText.text = InputManager.Instance.CameraLeft.ToString();
                break;
            case "CameraRight":
                cameraRightText.text = InputManager.Instance.CameraRight.ToString();
                break;
            case "Pause":
                pauseText.text = InputManager.Instance.Pause.ToString();
                break;
            case "AutoFire":
                autoFireText.text = InputManager.Instance.AutoFire.ToString();
                break;
        }
    }

    private void OnBackButtonClicked() {
        mainMenu.GetComponent<MenuUI>().BackToMainMenu();
        Hide();
    }
    
    
    
    public void Show() {
        controlChangePanel.SetActive(true);
        UpdateAllKeybindTexts();
    }
    public void Hide() {
        controlChangePanel.SetActive(false);
    }
    
}
