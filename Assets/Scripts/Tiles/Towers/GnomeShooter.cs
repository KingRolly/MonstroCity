using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class GnomeShooter : TowerTile
{
    public override IEnumerator DoAttack()
    {
        yield return new WaitForSeconds(2);

        FindNearestEnemy().GetComponent<Enemy>().TakeDamage(1);
    }
}
