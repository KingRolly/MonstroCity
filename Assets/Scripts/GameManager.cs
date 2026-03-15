using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager for the overall game state
/// <br/> - Nicholas Liang (March 14th, 2026)
/// </summary>
public class GameManager : MonoBehaviour
{
    [field: SerializeField] public bool isPaused { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject pauseMenu;

    /// <summary>
    /// Pause game
    /// </summary>
    public void PauseGame()
    {
        // Pause game by freezing everything that uses time.Deltatime and disable player input
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    /// <summary>
    /// Resume game
    /// </summary>
    public void ResumeGame()
    {
        // Reset time scale and re-enable player input
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
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
