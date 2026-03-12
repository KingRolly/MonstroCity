using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for start game button to switch scenes
/// </summary>
public class StartGameButton : MonoBehaviour
{
    /// <summary>
    /// Loads the "Level" scene
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }
}
