using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class GnomeShooter : TowerTile
{

    [SerializeField] private EnemyManager enemyManager;
    
    public override void DoAttack()
    {
        FindNearestEnemy(new Vector2(getX(), getY()));
    }

    void FindNearestEnemy(Vector2 pos)
    {
        foreach (GameObject e in enemyManager.GetAliveEnemiesList())
        {
            e.GetComponent<Enemy>().TakeDamage(100);
        }
    }
}
