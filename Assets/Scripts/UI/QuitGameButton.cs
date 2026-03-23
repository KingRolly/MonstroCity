using UnityEngine;

/// <summary>
/// Basic script for quit button
/// <br/> - Nicholas Liang (March 16th, 2026)
/// </summary>
public class QuitGameButton : MonoBehaviour
{
    // Close the game
    public void QuitGame()
    {
        Debug.Log("Successfully quit game");
        Application.Quit();
    }
}
