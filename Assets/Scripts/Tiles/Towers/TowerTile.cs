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
    PhaseManager phaseManager;

    protected override void Start()
    {
        setPlaceable(false);
        Debug.Log(data.towerName);
    }

    void Update()
    {
        while (phaseManager.GetCurrentPhase() == "day")
        {
            DoAttackPattern();

        }
    }

    public void DoAttackPattern()
    {
        
    }
}
