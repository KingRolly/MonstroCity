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
    /// Finds and returns the enemy closest to this tower
    /// </summary>
    public GameObject FindNearestEnemy()
    {
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject e in enemyManager.GetAliveEnemiesList())
        {
            float distance = Vector2.Distance(transform.position, e.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = e;
            }
        }
        return nearest;
    }
}
