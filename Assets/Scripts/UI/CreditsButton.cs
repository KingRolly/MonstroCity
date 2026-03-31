using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for credits button to switch scenes
/// </summary>
public class CreditsButton : MonoBehaviour
{
    /// <summary>
    /// Loads the "Credits" scene
    /// </summary>
    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
