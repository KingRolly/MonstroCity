using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTile : Tile
{
    /*
        All towers are implemented with a modular system.
        Each possible attribute for a tower's stats and attacks will be hardcoded as options.
        These will be represented as serialized fields in the editor so that each tower's properties can be edited without code.
    */

    [SerializeField] int attackMode;
    /*
        0: loop
        1: random
    */
    // attacks: a list of attacks, with type (projectile, AOE, etc.), cooldown, damage and other effects
    [SerializeField] List<Attack> attacks = new List<Attack>();

    protected override void Start()
    {

    }
}
