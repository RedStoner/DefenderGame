﻿using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{

    public GameObject enemyPrefab;
    public int numberOfEnemies;
    public float spawnInterval = 10.0f;
    private float spawnCooldown = 0.0f;

    public override void OnStartServer()
    {
        if (isServer) {
            SpawnEnemies();
        }
    }
    private void Update()
    {
        if (isServer)
        {
            spawnCooldown += Time.deltaTime;
            if (spawnCooldown > spawnInterval)
            {
                SpawnEnemies();
                spawnCooldown -= spawnInterval;
            }
        }
    }
    private void SpawnEnemies()
    {

        for (int i = 0; i < numberOfEnemies; i++)
        {
            var spawnPosition = new Vector3(
                Random.Range(0.0f, 0.5f),
                0.0f,
                Random.Range(0.0f, 0.5f));

            var spawnRotation = Quaternion.Euler(
                0.0f,
                Random.Range(0, 180),
                0.0f);

            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(enemy);
        }
    }
}