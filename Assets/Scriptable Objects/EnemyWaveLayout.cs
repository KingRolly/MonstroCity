using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class Description:
// Data container used to create what the layout/makeup of a wave looks like
// A list of these scriptable objects is given to EnemyManager who will spawn in waves of enemies for the current round according to what's given
// - Nicholas Liang (Feb. 20th, 2026)

[CreateAssetMenu(fileName = "New EnemyWaveLayout", menuName = "EnemyWaveLayout")]
public class EnemyWaveLayout : ScriptableObject
{
    [Tooltip("Put an EnemyStats scriptable object here for whichever enemy to spawn")]
    public EnemyStats enemyToSpawn;

    [Tooltip("Number of enemies to spawn")]
    public int amount;

    [Tooltip("The time delay before first enemy spawns in (0 by default)")]
    public float initialTimeDelay = 0;

    [Tooltip("The time delay between each enemy spawning")]
    public float timeDelayBetweenSpawns;
}
