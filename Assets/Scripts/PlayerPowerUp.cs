using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour {
    private Dictionary<PowerUpData.PowerUpType, PowerUpEffect> activePowerUps =
        new Dictionary<PowerUpData.PowerUpType, PowerUpEffect>();

    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private ActivePowerUpsUI activePowerUpsUI;

    [SerializeField] private GameObject ActivePowerUpCanvas;
    
    private class PowerUpEffect {
        public PowerUpData data;
        public float remainingTime;
    }

    private void Awake() {
        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        activePowerUpsUI = GetComponent<ActivePowerUpsUI>();
    }

    public void ActivatePowerUp(PowerUpData powerUpData) {
        if (!activePowerUps.TryGetValue(powerUpData.type, out PowerUpEffect effect)) {
            effect = new PowerUpEffect();
            activePowerUps[powerUpData.type] = effect;
            
            ApplyPowerUpEffect(powerUpData, powerUpData.effectIncrease);
        }

        effect.data = powerUpData;
        effect.remainingTime = powerUpData.duration;
    }

    private void Update() {
        List<PowerUpData.PowerUpType> expiredPowerUps = new List<PowerUpData.PowerUpType>();
        
        foreach(var kvp in activePowerUps) {
            PowerUpEffect effect = kvp.Value;
            effect.remainingTime -= Time.deltaTime;
            if (effect.remainingTime <= 0) {
                expiredPowerUps.Add(kvp.Key);
            }
        }
        
        foreach(var type in expiredPowerUps) {
            RemovePowerUpEffect(type);
            activePowerUps.Remove(type);
        }
    }

    private void ApplyPowerUpEffect(PowerUpData powerUpData, float increase) {
        switch (powerUpData.type) {
            case PowerUpData.PowerUpType.RapidFire:
                playerAttack.ApplyRapidFire(increase);
                break;
            case PowerUpData.PowerUpType.DamageIncrease:
                playerAttack.ApplyDamageIncrease(increase);
                break;
            case PowerUpData.PowerUpType.SpeedBoost:
                playerMovement.ApplySpeedBoost();
                break;
            case PowerUpData.PowerUpType.Shield:
                playerStats.ApplyShield();
                break;
                
        }
        ActivePowerUpCanvas.GetComponent<ActivePowerUpsUI>().ShowPowerUp(powerUpData.imagePU, powerUpData.duration);
    }

    private void RemovePowerUpEffect(PowerUpData.PowerUpType type) {
        switch (type) {
            case PowerUpData.PowerUpType.RapidFire:
                playerAttack.RemoveRapidFire();
                break;
            case PowerUpData.PowerUpType.DamageIncrease:
                playerAttack.RemoveDamageIncrease();
                break;
            case PowerUpData.PowerUpType.SpeedBoost:
                playerMovement.RemoveSpeedBoost();
                break;
            case PowerUpData.PowerUpType.Shield:
                playerStats.RemoveShield();
                break;
        }
    }
    
    
}
