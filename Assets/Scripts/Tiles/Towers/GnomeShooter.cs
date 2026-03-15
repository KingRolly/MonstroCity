using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.U2D;

public class GnomeShooter : TowerTile
{
    [SerializeField] private int projectileSpeed;
    [SerializeField] private GameObject gnomePrefab;
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
                    //Spawn eight gnome projectiles which will damage enemies
                    SpawnGnomes();
                }
            }
        }
    }

    private void SpawnGnomes()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject proj = Instantiate(gnomePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            proj.GetComponent<GnomeProjectile>().SetProjectileInfo(projectileSpeed, data.damage, data.attackRange, i*45);
        }
    }
}
