using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class Description:
// Handles spawning of enemies by directing spawn patterns for each wave
// - Nicholas Liang (Feb. 11th, 2026)
public class EnemyManager : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;
    public GameObject enemyPrefab;

    // [Header("Private Variables")]
    private List<GameObject> totalEnemies;
    private List<GameObject> aliveEnemies;
    private List<GameObject> deadEnemies;


    // Start is called before the first frame update
    void Start()
    {
        totalEnemies = new List<GameObject>();
        aliveEnemies = new List<GameObject>();
        deadEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Test spawning 5 enemies
    public void testSpawnEnemy()
    {
        StartCoroutine(spawnEnemies(1, 1f));
        //foreach (Vector2Int coordinate in gridManager.getPath())
        //{
        //    Debug.Log(coordinate);
        //}
    }

    // Spawns amt number of enemies with spawnDelay seconds between each spawn
    private IEnumerator spawnEnemies(int amt, float spawnDelay)
    {
        for (int i = 0; i < amt; i++)
        {
            // Instantiate enemy and set up its stats and pathfinding
            GameObject enemyToSpawn = Instantiate(enemyPrefab);
            enemyToSpawn.GetComponent<Enemy>().setStats("Peasant", 10, 1, 5);
            enemyToSpawn.GetComponent<Enemy>().setPath(gridManager.getPath());
            totalEnemies.Add(enemyToSpawn);

            // Delay between spawns
            yield return new WaitForSeconds(1);
        }
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
