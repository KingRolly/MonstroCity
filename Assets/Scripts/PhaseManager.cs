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
    [Header("Manager References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private GridManager gridManager;

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
    [SerializeField] private Tilemap bgTilemap;
    [SerializeField] private Tilemap pathTilemap;
    [SerializeField] private Material spriteMaterial;

    [Header("Phase Information")]
    [SerializeField] private string currentPhase;
    [SerializeField] private int dayCounter;
    [SerializeField] private int layoutIndex;
    // A list of wave layouts where each wave layout represents a day, combined they make up all days for a level
    [SerializeField] private List<EnemyWaveLayout> currentLevelEnemyWaveLayouts;
    

    private readonly Color32 NIGHT_TIME_COLOUR = new Color32(100, 100, 200, 255); // muted purplish tint
    private readonly Color32 DAY_TIME_COLOUR = new Color32(255, 255, 255, 255); // no tint whatsoever

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

        // Update counters
        StartCoroutine(SetPhase("Daytime"));
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
        // Update graphics
        readyButton.interactable = true;
        readyText.color = Color.red;

        // Update counters
        StartCoroutine(SetPhase("Night"));
        gridManager.ToggleEditing();
        layoutIndex++;
    }

    /// <summary>
    /// Set current phase
    /// <br/> REQUIRES: state is "Daytime" or "Night"
    /// </summary>
    /// <param name="state"></param>
    public IEnumerator SetPhase(string state)
    {
        
        // Update phase indicator text
        currentPhase = state;
        phaseIndicatorText.text = state;

        // Initialize variables for colour tinting
        Color32 currentBgColour = bgTilemap.color;
        Color32 currentPathColour = pathTilemap.color;
        Color32 currentSpriteMaterialColour = spriteMaterial.color;
        Color32 toColour;
        float t = 0.0f;
        float duration = 1.0f; // duration of colour tinting fade

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

        // Interpolate between colours to add tinting
        while (t < duration)
        {
            t += Time.deltaTime;
            //bgTilemap.color = Color32.Lerp(currentBgColour, toColour, t / time);
            //pathTilemap.color = Color32.Lerp(currentPathColour, toColour, t / time);
            spriteMaterial.color = Color32.Lerp(currentSpriteMaterialColour, toColour, t / duration);
            yield return null;
        }
    }

    /// <summary>
    /// Set day counter
    /// </summary>
    /// <param name="num"></param>
    public void SetDayCounter(int num)
    {
        dayCounter = num;
        dayCounterText.text = "Day " + num.ToString();
    }

    /// <summary>
    /// Incrememnt day counter
    /// </summary>
    private void IncrementDayCounter()
    {
        dayCounter++;
        dayCounterText.text = "Day " + dayCounter.ToString();
    }

    public string GetCurrentPhase()
    {
        return currentPhase;
    }
}
