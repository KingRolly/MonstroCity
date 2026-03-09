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
        if (phaseManager.GetCurrentPhase() == "Daytime")
        {
            DoAttack();

        }
    }

    /// <summary>
    /// Runs the attack for this tower
    /// </summary>
    public abstract void DoAttack();
}
