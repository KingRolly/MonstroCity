using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ArcherTower : TowerTile
{
    public override IEnumerator DoAttack()
    {
        yield return new WaitUntil(() => phaseManager.GetCurrentPhase() == "Daytime");
        FindNearestEnemy().GetComponent<Enemy>().TakeDamage(1000);

        yield return new WaitForSeconds(3);
        
    }
}
