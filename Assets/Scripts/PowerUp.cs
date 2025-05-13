using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    [HideInInspector] public PowerUpData data;
    [SerializeField] private Image front;
    [SerializeField] private Image back;
    [SerializeField] private Image right;
    [SerializeField] private Image left;
    
    private float speed = 2f;
    private float amplitude = 0.35f;
    private Vector3 startPos;
    
    private float lifeTime = 15f;
    void Start()
    {
        startPos = transform.position;
        Destroy(gameObject, lifeTime);
    }

    private void Update() {
        Movement();
    }
    

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            ApplyPowerUp(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void ApplyPowerUp(GameObject player) {
        PlayerPowerUp manager = player.GetComponent<PlayerPowerUp>();
        if (manager != null) {}
        manager.ActivatePowerUp(data);
    }

    public void SetSprite(Sprite newSprite) {
        front.sprite = newSprite;
        back.sprite = newSprite;
        right.sprite = newSprite;
        left.sprite = newSprite;
    }

    private void Movement() {
        float offsetY = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPos + new Vector3(0, offsetY, 0);
        transform.Rotate(0,20*Time.deltaTime,0);
    }
}
