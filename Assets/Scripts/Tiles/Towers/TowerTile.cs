using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a simple tower that has no unique behaviors
/// - Jack Peters (Feb. 27th, 2026)
/// </summary>
public abstract class TowerTile : Tile
{   
    [SerializeField] public TowerData data;
    [SerializeField] public EnemyManager enemyManager;
    [SerializeField] public PhaseManager phaseManager;

    void Awake()
    {
        phaseManager = FindObjectOfType<PhaseManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    protected override void Start()
    {
        setPlaceable(false);
        Debug.Log(data.towerName);
        StartCoroutine(AttackCycle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Runs the attack for this tower
    /// </summary>
    public abstract IEnumerator AttackCycle();

    /// <summary>
    /// Finds and returns the enemy closest to this tower within a max distance
    /// </summary>
    public GameObject FindNearestEnemy(double maxDistance)
    {
        GameObject nearest = null;
        double minDistance = maxDistance;
        foreach (GameObject e in enemyManager.GetAliveEnemiesList())
        {
            double distance = Vector2.Distance(transform.position, e.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = e;
            }
        }
        return nearest;
    }

    /// <summary>
    /// Finds and returns the enemy in range of the tower that is furthest along the path
    /// </summary>
    /// <returns></returns>
    public GameObject FindEnemyClosestToExit()
    {
        // TODO: Loop through list of enemies in range of tower and keep track of enemy with highest distanceTravelled
        //       Can call this instead of FindNearestEnemy to replace close targeting with first targeting for tower
        return null; //stub
    }

    /// <summary>
    /// Adds enemy to list of enemies in range of the tower
    /// </summary>
    public void AddInRangeEnemy(GameObject enemy)
    {
        // TODO: Implement a new child game object under tower that detects enemies within the towers range
        //       Child object calls this to make tower keep track of enemies in range (using OnTriggerEnter2D)
    }

    /// <summary>
    /// Removes enemy from list of enemies in range of the tower
    /// </summary>
    public void RemoveInRangeEnemy(GameObject enemy)
    {
        // TODO: Implement a new child game object under tower that detects enemies within the towers range
        //       Child object calls this to make tower remove an enemy no longer in range of the tower (using OnTriggerExit2D)
        //       Search for the enemy to remove by comparing their GameObject.GetInstanceID()
    }
}
