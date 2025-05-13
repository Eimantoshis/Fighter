using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour {

    [SerializeField] private GameObject deathUI;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button backToMainMenuButton;
    
    public static DeathUI Instance;

    private void Awake() {
        Instance = this;
    }
    
    
    // Start is called before the first frame update
    void Start() {
        Hide();
        playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        backToMainMenuButton.onClick.AddListener(OnBackToMainMenuButtonClicked);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlayAgainButtonClicked() {
        GameStateManager.Instance.ReloadGameScene();
    }

    private void OnBackToMainMenuButtonClicked() {
        
    }

    private void Hide() {
        deathUI.SetActive(false);
    }

    public void Show() {
        deathUI.SetActive(true);
    }
    
    
}
