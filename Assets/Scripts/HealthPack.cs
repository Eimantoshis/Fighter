using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [HideInInspector] public HealthPackData data;
    private float speed = 2f;
    private float amplitude = 0.35f;
    private Vector3 startPos;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerStats playerHealth = other.GetComponent<PlayerStats>();
            if (playerHealth != null) {
                playerHealth.Heal(data.healAmount);

                Destroy(gameObject);
            }
        }
    }

    void Start() {
        startPos = transform.position;
        Destroy(gameObject, 20f);
    }

    void Update() {
        float offsetY = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPos + new Vector3(0, offsetY, 0);
    }
}
