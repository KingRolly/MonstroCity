using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PathTile : Tile
{
    private float EnemySpeed;
    private float EnemyDamage;

    protected override void Start()
    {
        base.Start();
        setPlaceable(false);
        EnemySpeed = 0;
        EnemyDamage = 0;
    }
}
