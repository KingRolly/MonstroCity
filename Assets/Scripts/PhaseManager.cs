using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages day and night cycle within a level
/// - Jack Peters (Mar. 1st, 2026)
/// </summary>
public class PhaseManager : MonoBehaviour
{
    #region Fields
    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EnemyManager enemyManager;

    [Header("Phase Information")]
    [SerializeField] private string currentPhase;
    [SerializeField] private int dayCounter;
    [SerializeField] private EnemyWaveLayout[] currentLevelEnemyWaveLayouts;
    [SerializeField] private int layoutIndex;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentPhase = "night";
        dayCounter = 0;
        layoutIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginLevel()
    {
        currentPhase = "day";
        if (currentLevelEnemyWaveLayouts != null)
        {
            enemyManager.SpawnWave(currentLevelEnemyWaveLayouts[0]);
        }
    }

    public string GetCurrentPhase()
    {
        return currentPhase;
    }

    public void SetPhase(string state)
    {
        currentPhase = state;
    }
}
