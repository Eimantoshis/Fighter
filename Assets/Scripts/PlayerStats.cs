using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float health;
    private float XP = 0;
    private int level = 1;
    private float levelDamage = 5f;
    private float levelShootTimeIncrease= 0.05f;
    private float levelBulletSpeed = 0.1f;
    private float levelMaxHealth = 5f;
    private float levelMovementSpeed = 0.01f;
    public static event EventHandler onPlayerDamaged;

    private bool shieldOn = false; 
    //private Animator animator;

    private void Awake() {
        EnemyHP.OnEnemyDeath += HandleEnemyDeath;
    }

    private void OnDestroy() {
        EnemyHP.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(object sender, EnemyDeathEventArgs e) {
        GetXP(e.XPReward);
    }
    
    


    void Start()
    {
        //animator = gameObject.GetComponent<Animator>();
        health = maxHealth;
        gameObject.GetComponent<PlayerUI>().UpdateHealthBar();
    }

    public int GetLevel() {
        return level;
    }
    public float GetXP() {
        return XP;
    }
    public float GetHealth() {
        return health;
    }
    public float GetMaxHealth() {
        return maxHealth;
    }






    public void TakeDamage(float damage) {
        if (shieldOn) return;
        
        health -= damage;
        if (health < 0) {
            health = 0;
            Death();
        }
        gameObject.GetComponent<PlayerUI>().UpdateHealthBar();
        onPlayerDamaged?.Invoke(this, EventArgs.Empty);
        AudioManager.Instance.PlayPlayerHit();
        //animator.Play("PlayerGetsHit", -1, 0);

    }

    public void Heal(float amount) {
        health += amount;
        if (health > maxHealth) {
            health = maxHealth;
        }
        gameObject.GetComponent<PlayerUI>().UpdateHealthBar();
    }

    private void Death() {
        if (health <= 0) {
            AudioManager.Instance.PlayPlayerDeath();
            Time.timeScale = 0f;
            DeathUI.Instance.Show();
        }
    }



    public void GetXP(float amount) {
        XP += amount;
        if (XP >= 100) {
            LevelUp();
            XP -= 100;
        }
        gameObject.GetComponent<PlayerUI>().UpdateXPBar();
    }

    private void LevelUp() {
        level++;
        gameObject.GetComponent<PlayerUI>().LevelUp();
        gameObject.GetComponent<PlayerMovement>().IncreaseStatsOnLevel(levelMovementSpeed);
        gameObject.GetComponent<PlayerAttack>().IncreaseStatsOnLevel(levelDamage,levelBulletSpeed, levelShootTimeIncrease);
        maxHealth += levelMaxHealth;
        
    }


    public void ApplyShield() {
        shieldOn = true;
    }
    
    public void RemoveShield() {
        shieldOn = false;
    }
}
