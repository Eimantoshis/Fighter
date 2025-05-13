using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform player;
    [SerializeField] private int enemiesOnFirstWave = 7;
    private BoxCollider spawnArea;
    private EnemyWaveUI enemUI;

    private float minDistanceFromPlayer = 20f;
    private float waveTimer;
    private int wave = 1;
    private int aliveEnemies = 0;
    private bool waveInProgress = false;

    // Start is called before the first frame update

    private void Awake() {
        EnemyHP.OnEnemyDeath += HandleEnemyDeath;
    }

    private void OnDestroy() {
        EnemyHP.OnEnemyDeath -= HandleEnemyDeath;
    }

    void Start()
    {
        waveTimer = 2;
        spawnArea = GetComponent<BoxCollider>();
        enemUI = gameObject.GetComponent<EnemyWaveUI>();

    }

    private void HandleEnemyDeath(object sender, EventArgs e) {
        aliveEnemies--;
        if (aliveEnemies <= 0) {
            waveInProgress = false;
            GetComponent<EnemyWaveUI>().DisplayMessage("Wave cleared!");
            waveTimer = 5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnWave();
    }

    private void SpawnWave() {
        if (waveInProgress) return; // don't spawn until wave is cleared

        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0) {


            enemUI.DisplayMessage("Wave " + wave);
            int spawnCount = wave + enemiesOnFirstWave;
            aliveEnemies = spawnCount;
            waveInProgress = true;

            for (int i = 0; i < spawnCount; i++) {
                int index = UnityEngine.Random.Range(0, enemyPrefabs.Length);
                Instantiate(enemyPrefabs[index],GetRandomPositionInSpawnArea(), Quaternion.identity);
            }
            wave++;
        }
    }

    Vector3 GetRandomPositionInSpawnArea() {
        Vector3 center = spawnArea.bounds.center;
        Vector3 size = spawnArea.bounds.size;

        Vector3 spawnPos;

        do {
            float x = UnityEngine.Random.Range(center.x - size.x / 2, center.x + size.x / 2);
            float z = UnityEngine.Random.Range(center.z - size.z / 2, center.z + size.z / 2);
            spawnPos = new Vector3(x,1,z);
        } while (Vector3.Distance(spawnPos, player.position) <minDistanceFromPlayer);

        return spawnPos;
    }
}
