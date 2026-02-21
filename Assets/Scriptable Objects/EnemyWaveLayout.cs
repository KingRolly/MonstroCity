using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Data container used to create what the layout/makeup of a wave looks like <br/>
/// Stores a list of enemy spawn patterns combined to make up the layout of a wave <br/>
/// - Nicholas Liang (Feb. 20th, 2026)
/// </summary>

[CreateAssetMenu(fileName = "New EnemyWaveLayout", menuName = "EnemyWaveLayout")]
public class EnemyWaveLayout : ScriptableObject
{
    [Serializable]
    public class EnemySpawnPattern
    {
        [Tooltip("Put an EnemyStats scriptable object here for whichever enemy to spawn")]
        public EnemyStats enemyToSpawn;

        [Tooltip("Number of enemies to spawn")]
        public int amount;

        [Tooltip("The time delay before first enemy spawns in")]
        public float initialTimeDelay;

        [Tooltip("The time delay between each enemy spawning")]
        public float timeDelayBetweenSpawns;
    }

    [SerializeField] public EnemySpawnPattern[] spawnPatterns;
}
