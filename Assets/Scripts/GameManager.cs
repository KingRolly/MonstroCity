using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Manager for the overall game state
/// <br/> - Nicholas Liang (March 14th, 2026)
/// </summary>
public class GameManager : MonoBehaviour
{
    [field: SerializeField] public bool isPaused { get; private set; }
    public delegate void OnPause();
    public static OnPause onPause;
    public delegate void OnResume();
    public static OnResume onResume;

    [Header("References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI daySurvivalCounter;

    [Header("Manager References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PhaseManager phaseManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private MouseManager mouseManager;

    [Header("Pausable Components")]
    [SerializeField] private GameObject indicator;

    /// <summary>
    /// Pause game
    /// </summary>
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
        onPause?.Invoke();

        // Freeze time to stop time based actions
        Time.timeScale = 0;

        // Disable game objects and components who's functionality doesn't depend on time
        indicator.SetActive(false);
        AudioListener.pause = true;
    }

    /// <summary>
    /// Resume game
    /// </summary>
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        onResume?.Invoke();

        // Reset time scale and re-enable everything
        Time.timeScale = 1;
        indicator.SetActive(true);
        AudioListener.pause = false;
    }

    /// <summary>
    /// Swaps pause state
    /// </summary>
    public void SwapPauseState()
    {
        // Swap the pause state accordingly
        if (isPaused) 
        {
            // Paused -> unpaused
            ResumeGame();
        }
        else
        {
            // Unpaused -> paused
            PauseGame();
        }
    }

    /// <summary>
    /// Restart the current level
    /// </summary>
    public void RestartLevel()
    {
        ResumeGame();
        SceneManager.LoadScene("Level");
    }

    /// <summary>
    /// Quit the current level and go back to the main menu
    /// </summary>
    public void QuitLevel()
    {
        ResumeGame();
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Trigger a game over for the current level
    /// </summary>
    public void GameOver()
    {
        isPaused = true;
        onPause?.Invoke();

        // Freeze time to stop time based actions
        Time.timeScale = 0;

        // Disable game objects and components who's functionality doesn't depend on time
        indicator.SetActive(false);
        AudioListener.pause = true;

        // Display game over screen
        gameOverScreen.SetActive(true);
        if (phaseManager.dayCounter == 1)
        {
            daySurvivalCounter.text = $"You survived 1 day";
        }
        else
        {
            daySurvivalCounter.text = $"You survived {phaseManager.dayCounter} days";
        }
    }
}
