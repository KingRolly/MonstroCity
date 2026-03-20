using System.Collections;
using UnityEngine;

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
                yield return new WaitUntil(() => FindNearestEnemy(data.attackRange) != null);
                //Spawn eight gnome projectiles which will damage enemies
                SpawnGnomes();
                yield return new WaitForSeconds(data.attackSpeed);
            }
        }
    }

    private void SpawnGnomes()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject proj = Instantiate(gnomePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            proj.GetComponent<Projectile>().SetProjectileInfo(projectileSpeed, data.damage, data.attackRange, i*45);
        }
    }
}
