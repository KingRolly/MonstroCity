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
        // Stop menu background music to allow for level background music to play
        AudioSource bgmPlayer = BGMPlayer.instance.GetComponent<AudioSource>();
        bgmPlayer.Stop();
        bgmPlayer.clip = null;
        SceneManager.LoadScene("Level");
    }
}
