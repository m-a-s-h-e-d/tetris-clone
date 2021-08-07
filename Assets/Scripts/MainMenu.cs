using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Switches the scene to the game scene
    public void PlayGame()
    {
        // TODO Multiplayer functionality, there should be an option to start a new lobby and enter a lobby join code
        // Will most likely require locally hosted client servers
        SceneManager.LoadScene("Game");
    }

    // Opens the settings menu scene
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    // Closes the application
    public void QuitGame()
    {
        Application.Quit();
    }
}
