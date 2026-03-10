using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ArcherTower : TowerTile
{
    public override IEnumerator DoAttack()
    {
        yield return new WaitUntil(() => enemyManager.GetAliveEnemiesCount() > 0);
        yield return new WaitUntil(() => phaseManager.GetCurrentPhase() == "Daytime");
        while (enemyManager.GetAliveEnemiesCount() > 0)
        {
            yield return new WaitForSeconds(data.attackSpeed);
            Debug.Log("hi");
            FindNearestEnemy().GetComponent<Enemy>().TakeDamage((int) data.damage);
        }
    }
}
