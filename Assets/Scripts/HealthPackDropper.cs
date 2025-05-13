using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackDropper : MonoBehaviour
{
    [SerializeField] private HealthPackData[] possibleDrops;
    [SerializeField] GameObject healthPackPrefab;
    private int index;

    public void DropHealthPack(Vector3 position) {

        float roll = Random.Range(0f, 100f);
        if (roll < 60f) {
            // 60% no drop
            return;
        }
        else if (roll < 100f - 15f) {
            index = 0;
        }
        else if (roll < 100f - 5f) {
            index = 1;
        }
        else if (roll < 100f) {
            index = 2;
        }

        if (possibleDrops.Length == 0 || healthPackPrefab == null) {
            return;
        }
        HealthPackData selectedData = possibleDrops[index];

        GameObject spawned = Instantiate(healthPackPrefab, position, Quaternion.identity);
        spawned.GetComponent<HealthPack>().data = selectedData;
        spawned.GetComponent<Renderer>().material.color = selectedData.color;
    }
}
