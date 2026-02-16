using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Class Description:
// Handles spawning of enemies by directing spawn patterns for each wave
// - Nicholas Liang (Feb. 11th, 2026)
public class EnemyManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private EnemyStats peasantStats;
    [SerializeField] private EnemyStats knightStats;

    // [Header("Private Variables")]
    private List<GameObject> totalEnemies;
    private List<GameObject> aliveEnemies;
    private List<GameObject> deadEnemies;

    private IObjectPool<GameObject> enemyObjectPool;
    private bool collectionCheck = true;
    [Header("Object Pool Info")]
    [SerializeField] private int defaultCapacity = 25;
    [SerializeField] private int maxSize = 100;

    // Start is called before the first frame update
    void Start()
    {
        totalEnemies = new List<GameObject>();
        aliveEnemies = new List<GameObject>();
        deadEnemies = new List<GameObject>();
    }

    private void Awake()
    {
        // Setup enemy object pool
        enemyObjectPool = new ObjectPool<GameObject>(CreateEnemy, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, 
            collectionCheck, defaultCapacity, maxSize);
    }

    // Invoked when creating an enemy to populate the object pool
    private GameObject CreateEnemy()
    {
        GameObject enemyInstance = Instantiate(enemyPrefab);
        enemyInstance.GetComponent<Enemy>().enemyObjectPool = this.enemyObjectPool;
        return enemyInstance;
    }

    // Invoked when getting the next item from the object pool
    private void OnGetFromPool(GameObject pooledEnemyObject)
    {
        pooledEnemyObject.SetActive(true);
    }

    // Invoked when returning an item to the object pool
    private void OnReleaseToPool(GameObject pooledEnemyObject)
    {
        pooledEnemyObject.SetActive(false);
    }

    // Invoked when maximum number of pooled items gets exceeded
    private void OnDestroyPooledObject(GameObject pooledEnemyObject)
    {
        Destroy(pooledEnemyObject);
    }

    // Test spawning a peasant
    public void testSpawnPeasantEnemy()
    {
        StartCoroutine(spawnEnemies(peasantStats, 1, 1f));
    }

    // Test spawning a knight
    public void testSpawn5KnightEnemies()
    {
        StartCoroutine(spawnEnemies(knightStats, 5, 0.3f));
    }

    // Spawns amt number of enemies with spawnDelay seconds between each spawn
    private IEnumerator spawnEnemies(EnemyStats enemyStats, int amt, float spawnDelay)
    {
        for (int i = 0; i < amt; i++)
        {
            GameObject enemyToSpawn = enemyObjectPool.Get();

            // Get stats from enemyStats scriptable object
            string enemyType = enemyStats.enemyType;
            int health = enemyStats.health;
            float speed = enemyStats.speed;
            int damage = enemyStats.damage;

            // Set up enemy stats and pathfinding
            enemyToSpawn.GetComponent<Enemy>().setStats(enemyType, health, speed, damage);
            enemyToSpawn.GetComponent<Enemy>().setPath(gridManager.getPath());

            // Keep track of spawned enemy
            totalEnemies.Add(enemyToSpawn);
            aliveEnemies.Add(enemyToSpawn);

            // Delay between spawns
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Delete the given enemy and update the lists keeping track of enemies
    public void despawnEnemy(GameObject enemyToDespawn)
    {
        foreach (GameObject enemy in aliveEnemies)
        {
            if (enemyToDespawn.GetInstanceID() == enemy.GetInstanceID())
            {
                enemyObjectPool.Release(enemyToDespawn);
                deadEnemies.Add(enemy);
                aliveEnemies.Remove(enemy);
                return;
            }
        }

        Debug.Log("Could not find enemy");
    }

    // Basic getters
    public List<GameObject> getTotalEnemiesList()
    {
        return this.totalEnemies;
    }
    public List<GameObject> getAliveEnemiesList()
    {
        return this.aliveEnemies;
    }
    public List<GameObject> getDeadEnemiesList()
    {
        return this.deadEnemies;
    }
    public int getTotalEnemiesCount()
    {
        return this.totalEnemies.Count;
    }
    public int getAliveEnemiesCount()
    {
        return this.aliveEnemies.Count;
    }
    public int getDeadEnemiesCount()
    {
        return this.deadEnemies.Count;
    }
}
