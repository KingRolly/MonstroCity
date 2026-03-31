using System.Collections;
using UnityEngine;

public class GnomeShooter : TowerTile
{
    [Header("Gnome Shooter Info")]
    [SerializeField] private int projectileSpeed;
    [SerializeField] private GameObject gnomePrefab;

    [Header("Gnome Shooter SFX")]
    [SerializeField] private AudioClip attackSFX;

    [Header("Particles")]
    [SerializeField] private ParticleSystem leftCannonSmoke;
    [SerializeField] private ParticleSystem centreLeftCannonSmoke;
    [SerializeField] private ParticleSystem centreCannonSmoke;
    [SerializeField] private ParticleSystem centreRightCannonSmoke;
    [SerializeField] private ParticleSystem rightCannonSmoke;


    public override IEnumerator AttackCycle()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemyManager.GetAliveEnemiesCount() > 0); // wait until enemies have spawned
            while (enemyManager.GetAliveEnemiesCount() > 0) // keep running attack loop until all enemies are dead
            {
                // wait until an enemy is in range to initiate attack
                yield return new WaitUntil(() => FindNearestEnemy(data.attackRange) != null);

                // Activate smoke puff particles
                ActivateParticles();

                //Spawn eight gnome projectiles which will damage enemies
                SpawnGnomes();

                // Attack cooldown
                yield return new WaitForSeconds(data.attackSpeed); 
            }
        }
    }

    private void SpawnGnomes()
    {
        AudioManager.instance.PlaySoundFX(attackSFX, transform, 0.4f);
        for (int i = 0; i < 8; i++)
        {
            GameObject proj = Instantiate(gnomePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            proj.GetComponent<Projectile>().SetProjectileInfo(projectileSpeed, data.damage, data.attackRange, i*45);
        }
    }

    /// <summary>
    /// Play smoke puff particles at ends of gnome shooter's cannons
    /// </summary>
    private void ActivateParticles()
    {
        leftCannonSmoke.Play();
        centreLeftCannonSmoke.Play();
        centreCannonSmoke.Play();
        centreRightCannonSmoke.Play();
        rightCannonSmoke.Play();
    }
}
