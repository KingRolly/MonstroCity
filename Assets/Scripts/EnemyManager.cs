using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private UIManager uiManager;

    [Header("Scriptable objects for testing")]
    [SerializeField] private EnemyWaveLayout onePeasant;
    [SerializeField] private EnemyWaveLayout fiveKnights_LowCD;
    [SerializeField] private EnemyWaveLayout testWave;

    // [Header("Private Variables")]
    private List<GameObject> totalEnemies;
    private List<GameObject> aliveEnemies;
    private List<GameObject> deadEnemies;

    [Header("Enemy Wave Layout")]
    [SerializeField] private EnemyWaveLayout currentDayWaveLayout;

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
        SetCurrentDayWaveLayout(onePeasant);
        InitiateSpawning();
    }

    // Test spawning 5 knights with low spawn cooldown
    public void testSpawn5KnightEnemies()
    {
        SetCurrentDayWaveLayout(fiveKnights_LowCD);
        InitiateSpawning();
    }

    // Test spawning multiple spawn matterns to simulate a wave
    public void testSpawnRound()
    {
        SetCurrentDayWaveLayout(testWave);
        InitiateSpawning();
    }
    #endregion

    #region Enemy Spawning Functions
    /// <summary>
    /// Spawn a list of wave of enemies for given EnemyWaveLayout list
    /// </summary>
    /// <param name="layout">EnemyWaveLayout to spawn</param>
    public void SpawnWaves(EnemyWaveLayout layout)
    {
        // Assign list of wave layouts
        SetCurrentDayWaveLayout(layout);

        // Call private helper to start spawning
        InitiateSpawning();
    }

    /// <summary>
    /// Private helper for SpawnWaves() to initiate spawning all the wave layouts for current day
    /// </summary>
    private void InitiateSpawning()
    {
        // Reset lists tracking enemies
        totalEnemies.Clear();
        aliveEnemies.Clear();
        deadEnemies.Clear();

        // Check for empty wave layout list
        if (currentDayWaveLayout == null)
        {
            Debug.Log("There are no waves to spawn for current day");
            return;
        }

        // Start spawning
        StartCoroutine(SpawnAllEnemiesForCurrentWaveLayout());
    }

    /// <summary>
    /// Spawns in the enemies for the given current wave layout
    /// </summary>
    private IEnumerator SpawnAllEnemiesForCurrentWaveLayout()
    {
        // Check if layout for current wave layout is empty
        if (currentDayWaveLayout == null)
        {
            Debug.Log("Nothing to spawn for current wave layout");
            yield break;
        }

        
        // Read through layout of the current wave
        for (int i = 0; i < currentDayWaveLayout.spawnPatterns.Length; i++)
        {
            // Initial time delay of the spawn pattern
            yield return new WaitForSeconds(currentDayWaveLayout.spawnPatterns[i].initialTimeDelay);

            // Spawn enemies according to the given spawn pattern in wave layout
            StartCoroutine(SpawnEnemies(currentDayWaveLayout.spawnPatterns[i].enemyToSpawn, currentDayWaveLayout.spawnPatterns[i].amount, currentDayWaveLayout.spawnPatterns[i].timeDelayBetweenSpawns));
        }
    }

    /// <summary>
    /// Spawns in enemies according to given parameters
    /// </summary>
    /// <param name="enemyStats">Scriptable object represting the enemy to spawn</param>
    /// <param name="amt">Number of enemies to spawn</param>
    /// <param name="spawnDelay">Length of delay between each spawn</param>
    /// <returns></returns>
    private IEnumerator SpawnEnemies(EnemyStats enemyStats, int amt, float spawnDelay)
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
            enemyToSpawn.GetComponent<Enemy>().SetInfo(enemyType, health, speed, damage, sprite);
            enemyToSpawn.GetComponent<Enemy>().AssignReferences(this, uiManager);
            enemyToSpawn.GetComponent<Enemy>().SetPath(gridManager.getPath());

            // Keep track of spawned enemy
            totalEnemies.Add(enemyToSpawn);
            aliveEnemies.Add(enemyToSpawn);

            // Delay between spawns
            yield return new WaitForSeconds(spawnDelay);
        }
    }


    /// <summary>
    /// Assigns the wave layout for the current day
    /// </summary>
    /// <param name="waveLayout">Wave layout to assign</param>
    public void SetCurrentDayWaveLayout(EnemyWaveLayout waveLayout)
    {
        currentDayWaveLayout = waveLayout;
    }

    /// <summary>
    /// Delete the given enemy and update the lists keeping track of enemies
    /// </summary>
    /// <param name="enemyToDespawn"></param>
    public void DespawnEnemy(GameObject enemyToDespawn)
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
    public List<GameObject> GetTotalEnemiesList()
    {
        return this.totalEnemies;
    }
    public List<GameObject> GetAliveEnemiesList()
    {
        return this.aliveEnemies;
    }
    public List<GameObject> GetDeadEnemiesList()
    {
        return this.deadEnemies;
    }
    public int GetTotalEnemiesCount()
    {
        return this.totalEnemies.Count;
    }
    public int GetAliveEnemiesCount()
    {
        return this.aliveEnemies.Count;
    }
    public int GetDeadEnemiesCount()
    {
        return this.deadEnemies.Count;
    }
    #endregion
}
