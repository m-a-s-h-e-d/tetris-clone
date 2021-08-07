using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // UnityEngine audio mixer
    public AudioMixer audioMixer;

    // Dropdown menu for resolution options
    public TMPro.TMP_Dropdown resolutionDropdown;

    // Window resolutions
    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        // Create strings for resolutions and store current resolution index
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Add options to the dropdown menu then update selected resolution and refresh
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Set screen resolution
    public void setResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Set game volume based on slider value
    public void SetVolume(float volume)
    {
        // Use the audio mixer to handle volume
        audioMixer.SetFloat("volume", volume);
    }

    // Set game screen mode
    public void SetFullscreen(bool isFullscreen)
    {
        // Set the screen to the state of the toggle button
        Screen.fullScreen = isFullscreen;
    }

    // Closes the settings menu and returns to the main menu scene
    public void CloseSettings()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
