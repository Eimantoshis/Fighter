using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivePowerUpsUI : MonoBehaviour {
    [SerializeField] private GameObject ActivePowerUp;
    [SerializeField] private GameObject PowerUpsPanel;
    
    
    
    
    private void Update() {
        
    }


    public void ShowPowerUp(Sprite powerUpSprite, float duration) {
        GameObject newPowerUp = Instantiate(ActivePowerUp, PowerUpsPanel.transform);
        newPowerUp.transform.SetParent(PowerUpsPanel.transform, false);
            
        Transform powerUpIcon = newPowerUp.transform.Find("PowerUpIcon");
        if (powerUpIcon != null) {
            powerUpIcon.GetComponent<Image>().sprite = powerUpSprite;
        }
        
        Transform durationObject = newPowerUp.transform.Find("PowerUpDuration");
        if (durationObject != null) {
            durationObject.GetComponent<PowerUpDuration>().Initialize(duration);
        }

        
    }
}
