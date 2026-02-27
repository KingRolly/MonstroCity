using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerTile : Tile
{   
    private string towerType;
    private int price;
    private float damage;
    private float attackSpeed;
    private float attackRange;

    protected override void Start()
    {
        setPlaceable(false);
    }
}
