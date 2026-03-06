using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a simple tower that has no unique behaviors
/// - Jack Peters (Feb. 27th, 2026)
/// </summary>
public abstract class TowerTile : Tile
{   
    [SerializeField] TowerData data;
    public PhaseManager phaseManager;

    void Awake()
    {
        phaseManager = FindObjectOfType<PhaseManager>();
    }

    protected override void Start()
    {
        setPlaceable(false);
        Debug.Log(data.towerName);
    }

    // Update is called once per frame
    void Update()
    {
        while (phaseManager.GetCurrentPhase() == "Daytime")
        {
            DoAttackPattern();

        }
    }

    /// <summary>
    /// Runs a series of functions to carry out one "unit" of attack for this tower.
    /// </summary>
    public void DoAttackPattern()
    {
        
    }

    /// <summary>
    /// Shoots a projectile from this tower, with specified direction, damage, etc.
    /// </summary>
    public void FireProjectile(double damage, double angle)
    {
        
    }

    /// <summary>
    /// Deals damage to enemies within a range of this tower.
    /// </summary>
    public void AreaOfEffect(double damage, double range)
    {
        
    }

    /// <summary>
    /// Picks a path tile or tiles and sets them to do DOT for a specified duration.
    /// </summary>
    public void DamageOverTime(double damage, double duration)
    {
        
    }
}
