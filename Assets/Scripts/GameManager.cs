using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

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
        // TODO: Reset states, values, and scene
        ResumeGame();
        SceneManager.LoadScene("Level");
    }

    /// <summary>
    /// Quit the current level and go back to the main menu
    /// </summary>
    public void QuitLevel()
    {
        // TODO: Reset everything for when player comes back to level, then quit to main menu
        ResumeGame();
        SceneManager.LoadScene("Menu");
    }
}
