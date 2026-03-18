using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages day and night cycle within a level
/// - Jack Peters (Mar. 1st, 2026)
/// </summary>
public class PhaseManager : MonoBehaviour
{
    #region Fields
    [Header("References")]
    [SerializeField] private AudioClip readyButtonSound;

    [Header("Manager References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameManager gameManager;

    [Header("Phase Indicator References")]
    [SerializeField] private TextMeshProUGUI phaseIndicatorText;
    [SerializeField] private Image phaseIcon;
    [SerializeField] private Sprite daytimeIcon;
    [SerializeField] private Sprite nightIcon;

    [Header("Day Indicator References")]
    [SerializeField] private TextMeshProUGUI dayCounterText;

    [Header("Ready Button References")]
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI readyText;

    [Header("Graphics References")]
    [SerializeField] private Material spriteMaterial;
    [SerializeField] private GameObject materialReference;

    [field: Header("Level Information")]
    [field: SerializeField] public int dayCounter { get; private set; }
    [field: SerializeField] public int totalDaysInLevel { get; private set; }

    [Header("Phase Information")]
    [SerializeField] private string currentPhase;
    [SerializeField] private int layoutIndex;
    // A list of wave layouts where each wave layout represents a day, combined they make up all days for a level
    [SerializeField] private List<EnemyWaveLayout> currentLevelEnemyWaveLayouts;
    

    [SerializeField] private Color32 NIGHT_TIME_COLOUR; // muted purplish tint
    [SerializeField] private Color32 DAY_TIME_COLOUR; // no tint whatsoever

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        spriteMaterial.color = NIGHT_TIME_COLOUR;
        SetPhase("Night");
        SetDayCounter(0);
        layoutIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Called when ready button is clicked. Spawns in enemies for the current level and sets editing to false.
    /// </summary>
    public void BeginLevel()
    {
        if (gridManager.IsPathValid())
        {
            AudioManager.instance.PlaySound(readyButtonSound, transform, 0.7f);
            // Turn off editing and switch to day
            gridManager.ToggleEditing();
            StartDay();
        } else
        {
            // TODO: Indicate that path is invalid somehow
            Debug.Log("Path invalid!");
        }
    }

    /// <summary>
    /// Switch to daytime and spawn enemies
    /// </summary>
    private void StartDay()
    {
        // Update graphics
        readyButton.interactable = false;
        readyText.color = readyButton.colors.disabledColor;
        uiManager.HideTowerPanel();

        // Update counters
        SetPhase("Daytime");
        IncrementDayCounter();

        // Spawn enemies
        if (currentLevelEnemyWaveLayouts != null) // Check for non-empty list
        {
            // Call Enemy Manager to spawn waves for current day
            enemyManager.SpawnWaves(currentLevelEnemyWaveLayouts[layoutIndex]);
        }
    }

    /// <summary>
    /// End the day and switch to night
    /// </summary>
    public void EndDay()
    {
        // Check if this was the last day for the level
        if (dayCounter == totalDaysInLevel)
        {
            gameManager.TriggerLevelCompletion();
        }

        // Update graphics
        readyButton.interactable = true;
        readyText.color = Color.red;

        // Update counters
        SetPhase("Night");
        gridManager.ToggleEditing();
        layoutIndex++;
        uiManager.ShowTowerPanel();
    }

    /// <summary>
    /// Set current phase
    /// <br/> REQUIRES: state is "Daytime" or "Night"
    /// </summary>
    /// <param name="state"></param>
    public void SetPhase(string state)
    {
        // Update phase indicator text
        currentPhase = state;
        phaseIndicatorText.text = state;

        // Initialize variables for colour tinting
        Color32 currentSpriteMaterialColour = spriteMaterial.color;
        Color32 toColour;

        // Assign graphics according to given phase
        if (state == "Daytime")
        {
            phaseIcon.sprite = daytimeIcon;
            toColour = DAY_TIME_COLOUR;
            
        }
        else
        {
            phaseIcon.sprite = nightIcon;
            toColour = NIGHT_TIME_COLOUR;
        }

        float duration = 1.0f; // duration of colour tinting fade
        // Interpolate between colours to add tinting
        // Have to do it this way because you can't explicitly do .LeanColor on a material's colour tint
        LeanTween.value(gameObject, 0f, 1f, duration).setEaseInOutSine()
            .setOnUpdate((float val) => {
                // Lerp between colors based on tween value
                spriteMaterial.color = Color.Lerp(currentSpriteMaterialColour, toColour, val);
            });
    }

    /// <summary>
    /// Set day counter
    /// </summary>
    /// <param name="num"></param>
    public void SetDayCounter(int num)
    {
        dayCounter = num;
        dayCounterText.text = $"{num.ToString()}/{totalDaysInLevel.ToString()} ";
    }

    /// <summary>
    /// Incrememnt day counter
    /// </summary>
    private void IncrementDayCounter()
    {
        dayCounter++;
        dayCounterText.text = $"{dayCounter.ToString()}/{totalDaysInLevel.ToString()} ";
    }

    public string GetCurrentPhase()
    {
        return currentPhase;
    }
}
