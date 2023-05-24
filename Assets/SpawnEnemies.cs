using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnTime = 5f;
    public Transform spawnPoint;
    private float spawnTimer;

    void Start()
    {
        spawnTimer = Random.Range(15f, 20f);
        InvokeRepeating("SpawnEnemy", spawnTime, spawnTimer);
    }

    void SpawnEnemy()
    {
        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}
