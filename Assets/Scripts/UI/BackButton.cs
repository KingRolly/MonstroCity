using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for back button to switch scenes from credits to menu
/// </summary>
public class BackButton : MonoBehaviour
{
    /// <summary>
    /// Loads the "Menu" scene
    /// </summary>
    public void GoBack()
    {
        SceneManager.LoadScene("Menu");
    }
}
