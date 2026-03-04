using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField] private TextMeshProUGUI phaseIndicatorText;
    [SerializeField] private Image phaseIcon;
    [SerializeField] private Sprite daytimeIcon;
    [SerializeField] private Sprite nightIcon;

    [Header("Phase Information")]
    [SerializeField] private string currentPhase;
    [SerializeField] private int dayCounter;
    // A list of wave layouts where each wave layout represents a day, combined they make up all days for a level
    [SerializeField] private List<EnemyWaveLayout> currentLevelEnemyWaveLayouts;
    [SerializeField] private int layoutIndex;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SetPhase("Night");
        dayCounter = 0;
        layoutIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Called when ready button is clicked and spawns in enemies for the current level
    /// </summary>
    public void BeginLevel()
    {
        // TODO: Disable path placement and any other things that are supposed to be only available at the start of a level

        // Reset phase and day counter
        dayCounter = 0;
        layoutIndex = 0;

        // Switch to daytime
        StartDay();
    }

    /// <summary>
    /// Switch to daytime and spawn enemies
    /// </summary>
    private void StartDay()
    {
        SetPhase("Daytime");
        if (currentLevelEnemyWaveLayouts != null) // Check for non-empty list
        {
            // Call Enemy Manager to spawn waves for current day
            enemyManager.SpawnWaves(currentLevelEnemyWaveLayouts[layoutIndex]);
        }
    }

    public void EndDay()
    {
        // Update phase and counters
        SetPhase("Night");
        layoutIndex++;
        dayCounter++;
    }

    public string GetCurrentPhase()
    {
        return currentPhase;
    }

    /// <summary>
    /// Set current phase
    /// </summary>
    /// <param name="state"></param>
    public void SetPhase(string state)
    {
        currentPhase = state;
        phaseIndicatorText.text = state;
        if (state == "Daytime")
        {
            phaseIcon.sprite = daytimeIcon;
        }
        else
        {
            phaseIcon.sprite = nightIcon;
        }
        
    }
}
