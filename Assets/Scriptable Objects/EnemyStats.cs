using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class description:
// Data container for the stats of an enemy. Used to assign stats for instantiated enemies
// - Nicholas Liang (Feb. 20th, 2026)

[CreateAssetMenu(fileName = "New EnemyStats", menuName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public string enemyType;
    public Sprite sprite;
    public int health;
    public float speed;
    public int damage;
}
