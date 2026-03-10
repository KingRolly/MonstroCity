using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (ScriptableObject) Data container for the stats of an enemy. Used to assign stats for instantiated enemies <br/>
/// - Nicholas Liang (Feb. 20th, 2026)
/// </summary>


[CreateAssetMenu(fileName = "New EnemyStats", menuName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public string enemyType;
    public Sprite sprite;
    public int health;
    public float speed;
    public int damage;
    public int moneyReward;
}
