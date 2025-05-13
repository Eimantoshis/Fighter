using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private GameObject healthCanvas;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float healthMax = 100;
    [SerializeField] private float XPReward = 20f;
    private float health;
    private Vector3 HPBarAboveHead = new Vector3(0,1.5f,0);

    public static event EventHandler<EnemyDeathEventArgs> OnEnemyDeath;
    // Start is called before the first frame update
    void Start()
    {
        healthCanvas.SetActive(false);
        health = healthMax;
        UpdateHealthBar();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldPos = this.transform.position + HPBarAboveHead;
        healthSlider.transform.position = Camera.main.WorldToScreenPoint(worldPos);
    }


        private void UpdateHealthBar() {
        float hp = health / healthMax;
        if (hp < 1) {
            healthCanvas.SetActive(true);
        } else {
            healthCanvas.SetActive(false);
        }
        healthSlider.value = health / healthMax;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        UpdateHealthBar();
        if (health < 0) {
            health = 0;
        }
        Death();
    }

    private void Death() {
        if (health <= 0) {
            
            OnEnemyDeath?.Invoke(this, new EnemyDeathEventArgs(XPReward));

            var dropper = GetComponent<HealthPackDropper>();
            if (dropper != null) {
                dropper.DropHealthPack(transform.position);
            } else {
            }

            Destroy(gameObject);
        }
    }
}

public class EnemyDeathEventArgs : EventArgs {
    public float XPReward {get; private set;}
    public EnemyDeathEventArgs(float XPReward) {
        this.XPReward = XPReward;
    }
}
