using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyStats", menuName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public string enemyType;
    public int health;
    public float speed;
    public int damage;
}
