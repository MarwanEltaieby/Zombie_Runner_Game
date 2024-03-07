using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemySpawners;
    [SerializeField] GameObject parent;
    [SerializeField] float spawnCooldown;
    [SerializeField] int startingNumberOfZombies = 16;
    [SerializeField] TMP_Text waveText;
    float coolDown;
    int waveNum;
    int numOfZombiesPerWave;
    int numOfZombiesCreated;
    static int zombiesKilled;
    [SerializeField] GameObject zombie;

    private void Start() {
        coolDown = spawnCooldown;
        StartNewWaveSequence();
    }

    private void Update() {
        coolDown -= Time.deltaTime;
        if (coolDown <= 0 && numOfZombiesCreated < numOfZombiesPerWave) {
            SpawnZombies();
        }
        if (zombiesKilled == numOfZombiesPerWave) {
            StartNewWaveSequence();
        }
    }

    private void StartNewWaveSequence() {
        zombiesKilled = 0;
        waveNum++;
        waveText.SetText("Wave " + waveNum);
        coolDown = 10f;
        SetNumOfZombiesPerWave();
    }

    private void SetNumOfZombiesPerWave() {
        if (waveNum <= 13) {
            numOfZombiesPerWave = waveNum * startingNumberOfZombies;
            numOfZombiesCreated = 0;
            spawnCooldown = (spawnCooldown + 1f) / 2f;
        } else {
            numOfZombiesPerWave = (13 * startingNumberOfZombies) + (waveNum * 2); 
            numOfZombiesCreated = 0;
        }
    }

    private void SpawnZombies() {
        if (numOfZombiesCreated <= numOfZombiesPerWave) { 
            int spawnNum = Random.Range(0, 3);
            GameObject enemySpawner = enemySpawners[spawnNum];
            Instantiate(zombie, enemySpawner.transform.position, Quaternion.identity, parent.transform);
            coolDown = spawnCooldown;
            numOfZombiesCreated++;
        }
    }

    public static void IncrementKillCounter() {
        zombiesKilled++;
    }
}
