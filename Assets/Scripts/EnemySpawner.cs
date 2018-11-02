using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{

    public GameObject enemyPrefab;
    public int numberOfEnemies;

    public override void OnStartServer()
    {
        if (isServer) {
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
}