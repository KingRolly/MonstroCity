using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ArcherTower : TowerTile
{
    public override IEnumerator AttackCycle()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemyManager.GetAliveEnemiesCount() > 0);
            while (enemyManager.GetAliveEnemiesCount() > 0)
            {
                yield return new WaitForSeconds(data.attackSpeed);
                if (FindNearestEnemy(data.attackRange) != null)
                {
                    FindNearestEnemy(data.attackRange).GetComponent<Enemy>().TakeDamage((int) data.damage);
                }
            }
        }
    }
}
