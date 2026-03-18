using System.Collections;
using UnityEngine;

public class DaggerThrower : TowerTile
{
    [Header("Dagger Thrower Info")]
    [SerializeField] private int projectileSpeed;
    [SerializeField] private GameObject daggerPrefab;

    [Header("Dagger Thrower SFX")]
    [SerializeField] private AudioClip attackSFX;
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
                    Vector2 difference = FindNearestEnemy(data.attackRange).transform.position - transform.position;
                    float direction = Vector2.SignedAngle(Vector2.right, difference);
                    // Throw a dagger towards the enemy with given angle
                    ThrowDagger(direction);
                }
            }
        }
    }

    private void ThrowDagger(float direction)
    {
        AudioManager.instance.PlaySoundFX(attackSFX, transform, 0.5f);
        GameObject proj = Instantiate(daggerPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        proj.GetComponent<Projectile>().SetProjectileInfo(projectileSpeed, data.damage, data.attackRange, direction);
    }
}
