using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // UnityEngine audio mixer
    public AudioMixer AudioMixer;

    // Dropdown menu for resolution options
    public TMPro.TMP_Dropdown ResolutionDropdown;

    // Fullscreen button status
    public Toggle Toggle;

    // Window resolutions
    private Resolution[] _resolutions;

    private void Start()
    {
        // Set the full screen button to the current status
        Toggle.isOn = Screen.fullScreen;

        _resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions();

        // Create strings for resolutions and store current resolution index
        var options = new List<string>();
        var currentResolutionIndex = 0;
        for (var i = 0; i < _resolutions.Length; i++)
        {
            var option = _resolutions[i].width + " x " + _resolutions[i].height;

            if (_resolutions[i].refreshRate > 60)
            {
                option += " " + _resolutions[i].refreshRate + "hz";
            }

            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Add options to the dropdown menu then update selected resolution and refresh
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    // Set screen resolution
    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Set game volume based on slider value
    public void SetVolume(float volume)
    {
        // Use the audio mixer to handle volume
        AudioMixer.SetFloat("volume", volume);
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
