using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ArcherTower : TowerTile
{
    public override IEnumerator DoAttack()
    {
        yield return new WaitForSeconds(5);

        FindNearestEnemy().GetComponent<Enemy>().TakeDamage(5);
    }
}
