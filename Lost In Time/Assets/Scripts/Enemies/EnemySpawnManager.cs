using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawnManager : MonoBehaviour
{
    public List<GameObject> enemySpawnPoints;
    public List<Enemy> enemies;

    [SerializeField] private float startSpawnRate = 1f;  
    [SerializeField] private float maxSpawnRate = 0.5f;
    private float timeSinceLastSpawn;

    private float timeSinceLastIncrease;
    private float spawnRateIncreaseRate = 2f; // Spawn rate increases every 5 seconds
    private float spawnRateIncreaseAmount = 0.1f;
    private float currentSpawnRate;

    private void Awake()
    {
        currentSpawnRate = startSpawnRate;
    }

    private void Start()
    {
        timeSinceLastIncrease = Time.time;
        timeSinceLastSpawn = Time.time;
    }

    private void Update()
    {
        if (Time.time >= timeSinceLastSpawn + currentSpawnRate)
        {
            timeSinceLastSpawn = Time.time;

            SpawnEnemy();
        }

        IncreaseSpawnRate();
    }

    private void SpawnEnemy()
    {
        int spawnPosIndex = Random.Range(0, enemySpawnPoints.Count);
        int randomEnemyIndex = Random.Range(0, enemies.Count);

        Instantiate(enemies[randomEnemyIndex].gameObject, enemySpawnPoints[spawnPosIndex].transform.position, Quaternion.identity);
    }

    private void IncreaseSpawnRate()
    {
        if (Time.time < timeSinceLastIncrease + spawnRateIncreaseRate)
        {
            return;
        }

        currentSpawnRate -= spawnRateIncreaseAmount;
        if (currentSpawnRate < maxSpawnRate)
        {
            currentSpawnRate = maxSpawnRate;
        }

        timeSinceLastIncrease -= Time.time;
    }

}
