using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Handles spawning of enemies by directing spawn patterns for each wave <br/>
/// - Nicholas Liang (Feb. 11th, 2026)
/// </summary>
public class EnemyManager : MonoBehaviour
{
    #region Fields
    [Header("References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Scriptable objects for testing")]
    [SerializeField] private EnemyWaveLayout onePeasant;
    [SerializeField] private EnemyWaveLayout fiveKnights_LowCD;
    [SerializeField] private EnemyWaveLayout testWave;

    // [Header("Private Variables")]
    private List<GameObject> totalEnemies;
    private List<GameObject> aliveEnemies;
    private List<GameObject> deadEnemies;

    [Header("Enemy Wave Layout")]
    [SerializeField] private EnemyWaveLayout currentWaveLayout;

    // Object pool stuff
    private IObjectPool<GameObject> enemyObjectPool;
    private bool collectionCheck = true;
    [Header("Object Pool Info")]
    [SerializeField] private int defaultCapacity = 25;
    [SerializeField] private int maxSize = 100;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        totalEnemies = new List<GameObject>();
        aliveEnemies = new List<GameObject>();
        deadEnemies = new List<GameObject>();
    }

    #region Object Pool Functions
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
    #endregion

    #region Test Functions
    // Test spawning a peasant
    public void testSpawnPeasantEnemy()
    {
        setCurrentWaveLayout(onePeasant);
        StartCoroutine(spawnAllEnemiesForCurrentRound());
    }

    // Test spawning 5 knights with low spawn cooldown
    public void testSpawn5KnightEnemies()
    {
        setCurrentWaveLayout(fiveKnights_LowCD);
        StartCoroutine(spawnAllEnemiesForCurrentRound());
    }

    // Test spawning multiple spawn matterns to simulate a wave
    public void testSpawnRound()
    {
        setCurrentWaveLayout(testWave);
        StartCoroutine(spawnAllEnemiesForCurrentRound());
    }
    #endregion

    #region Enemy Spawning Functions
    /// <summary>
    /// Spawns in the enemies for the current wave
    /// </summary>
    private IEnumerator spawnAllEnemiesForCurrentRound()
    {
        // Check if layout for current wave layout is empty
        if (currentWaveLayout == null)
        {
            Debug.Log("Nothing to spawn for current round");
            yield break;
        }

        
        // Read through layout of the current wave
        for (int i = 0; i < currentWaveLayout.spawnPatterns.Length; i++)
        {
            // Initial time delay of the spawn pattern
            yield return new WaitForSeconds(currentWaveLayout.spawnPatterns[i].initialTimeDelay);

            // Spawn enemies according to the given EnemyWaveLayout
            StartCoroutine(spawnEnemies(currentWaveLayout.spawnPatterns[i].enemyToSpawn, currentWaveLayout.spawnPatterns[i].amount, currentWaveLayout.spawnPatterns[i].timeDelayBetweenSpawns));
        }
    }

    /// <summary>
    /// Spawns in enemies according to given parameters
    /// </summary>
    /// <param name="enemyStats">Scriptable object represting the enemy to spawn</param>
    /// <param name="amt">Number of enemies to spawn</param>
    /// <param name="spawnDelay">Length of delay between each spawn</param>
    /// <returns></returns>
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
            Sprite sprite = enemyStats.sprite;

            // Set up enemy stats and pathfinding
            enemyToSpawn.GetComponent<Enemy>().setInfo(enemyType, health, speed, damage, sprite, this);
            enemyToSpawn.GetComponent<Enemy>().setPath(gridManager.getPath());

            // Keep track of spawned enemy
            totalEnemies.Add(enemyToSpawn);
            aliveEnemies.Add(enemyToSpawn);

            // Delay between spawns
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    /// <summary>
    /// Set the enemy wave layout for the current wave
    /// </summary>
    /// <param name="wave"></param>
    public void setCurrentWaveLayout(EnemyWaveLayout wave)
    {
        currentWaveLayout = wave;
    }

    /// <summary>
    /// Delete the given enemy and update the lists keeping track of enemies
    /// </summary>
    /// <param name="enemyToDespawn"></param>
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
    #endregion

    #region Basic Getters
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
    #endregion
}
