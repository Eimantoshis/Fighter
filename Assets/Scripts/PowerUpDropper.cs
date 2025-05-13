using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDropper : MonoBehaviour {
    [SerializeField] private PowerUpData[] possibleDrops;
    [SerializeField] GameObject powerUpPrefab;
    
    private float minDistanceFromPlayer = 15f;
    [SerializeField] private Transform player;
    private BoxCollider spawnArea;
    private float powerUpTimer;
    private float powerUpTimerMax = 5f;

    private void Start() {
        spawnArea = GetComponent<BoxCollider>();
        powerUpTimer = powerUpTimerMax;
    }

    private void Update() {
        SpawnPowerUp();
    }
    

    public void DropPowerUp(Vector3 position) {
        int index = Mathf.FloorToInt(Random.Range(0, possibleDrops.Length));
        PowerUpData selectedData = possibleDrops[index];
        
        GameObject spawned = Instantiate(powerUpPrefab, position, Quaternion.identity);
        PowerUp powerUpScript = spawned.GetComponent<PowerUp>();
        powerUpScript.data = selectedData;
        powerUpScript.SetSprite(selectedData.imagePU);
    }
    
    private void SpawnPowerUp() {
        powerUpTimer -= Time.deltaTime;
        if (powerUpTimer <= 0) {
            DropPowerUp(GetRandomPositionInSpawnArea());
            powerUpTimer = powerUpTimerMax;
        }
    }
    
    
    Vector3 GetRandomPositionInSpawnArea() {
        Vector3 center = spawnArea.bounds.center;
        Vector3 size = spawnArea.bounds.size;

        Vector3 spawnPos;

        do {
            float x = UnityEngine.Random.Range(center.x - size.x / 2, center.x + size.x / 2);
            float z = UnityEngine.Random.Range(center.z - size.z / 2, center.z + size.z / 2);
            spawnPos = new Vector3(x,1.5f,z);
        } while (Vector3.Distance(spawnPos, player.position) < minDistanceFromPlayer);

        return spawnPos;
    }
}
