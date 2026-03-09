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
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private TextMeshProUGUI phaseIndicatorText;
    [SerializeField] private Image phaseIcon;
    [SerializeField] private Sprite daytimeIcon;
    [SerializeField] private Sprite nightIcon;
    [SerializeField] private TextMeshProUGUI dayCounterText;
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private Tilemap bgTilemap;
    [SerializeField] private Tilemap pathTilemap;

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
        SetPhase("Night");
        SetDayCounter(0);
        layoutIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (GetCurrentPhase() == "Night")
        //{
        //    readyButton.SetActive(true);
        //} else
        //{
        //    readyButton.SetActive(false);
        //}
    }

    /// <summary>
    /// Called when ready button is clicked and spawns in enemies for the current level
    /// </summary>
    public void BeginLevel()
    {
        // TODO: Disable path placement and any other things that are supposed to be only available at the start of a level

        // Switch to daytime
        StartDay();
    }

    /// <summary>
    /// Switch to daytime and spawn enemies
    /// </summary>
    private void StartDay()
    {
        // Update graphics
        readyButton.interactable = false;
        readyText.color = readyButton.colors.disabledColor;
        //bgTilemap.color = DAY_TIME_COLOUR;
        //pathTilemap.color = DAY_TIME_COLOUR;

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
        //bgTilemap.color = NIGHT_TIME_COLOUR;
        //pathTilemap.color = NIGHT_TIME_COLOUR;

        // Update counters
        StartCoroutine(SetPhase("Night"));
        layoutIndex++;
    }

    /// <summary>
    /// Set current phase
    /// </summary>
    /// <param name="state"></param>
    public IEnumerator SetPhase(string state)
    {
        
        // Update phase indicator text
        currentPhase = state;
        phaseIndicatorText.text = state;

        // Update indicator icon and graphics
        Color32 currentBgColour = bgTilemap.color;
        Color32 currentPathColour = pathTilemap.color;
        float t = 0.0f;
        float time = 0.2f;

        if (state == "Daytime")
        {
            phaseIcon.sprite = daytimeIcon;

            // Interpolate between colours on tilemap
            while (t < time)
            {
                t += Time.deltaTime;
                bgTilemap.color = Color32.Lerp(currentBgColour, DAY_TIME_COLOUR, t / time);
                pathTilemap.color = Color32.Lerp(currentPathColour, DAY_TIME_COLOUR, t / time);
                yield return null;
            }
        }
        else
        {
            phaseIcon.sprite = nightIcon;

            // Interpolate between colours on tilemap
            while (t < time)
            {
                t += Time.deltaTime;
                bgTilemap.color = Color32.Lerp(currentBgColour, NIGHT_TIME_COLOUR, t / time);
                pathTilemap.color = Color32.Lerp(currentPathColour, NIGHT_TIME_COLOUR, t / time);
                yield return null;
            }
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
