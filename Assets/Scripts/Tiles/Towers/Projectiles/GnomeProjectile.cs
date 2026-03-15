using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

/// <summary>
/// GnomeShooter projectile <br/>
/// - Jagger Shergill (Mar. 15th, 2026)
/// </summary>
public class GnomeProjectile : MonoBehaviour
{
    private float speed; // speed of projectile
    private float damage; // amount of damage projectile deals
    private float maxDistance; // max distance projectile travels before dissapearing
    private float directionAngle; // determines the direction projectile is travelling in
    private float distanceTravelled; // distance projectile has travelled so far

    // Start is called before the first frame update
    void Start()
    {
    
    }

    //Make sure to call this when spawning GnomeProjectile from GnomeShooter
    public void SetProjectileInfo(float speed, float damage, float maxDistance, float directionAngle)
    {
        this.speed = speed;
        this.damage = damage;
        this.maxDistance = maxDistance;
        this.directionAngle = directionAngle;

        distanceTravelled = 0;
        transform.rotation = Quaternion.Euler(0f, 0f, directionAngle);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * transform.right;
        distanceTravelled += speed * Time.deltaTime;

        if (distanceTravelled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Enemy"))
        {
            //Damage enemy
            collider.gameObject.GetComponent<Enemy>().TakeDamage((int) damage);

            //Optional, destroy projectile on collision (we want pierce though I think)
            // Destroy(gameObject);
        }
    }
}
