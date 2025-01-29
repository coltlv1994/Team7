//Bisma
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsSettings : MonoBehaviour


{

    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullScreen;

     void Start()

    {
        Resolution[] resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        var resolutionOptions = new System.Collections.Generic.List<string>();

        foreach (Resolution resolution in resolutions)
        {
            resolutionOptions.Add(resolution.width+"x"+ resolution.height); 
        }
        resolutionDropdown.AddOptions(resolutionOptions);

        string currentResolution=Screen.currentResolution.width + "x" + Screen.currentResolution.height;
        resolutionDropdown.value=resolutionOptions.IndexOf(currentResolution);

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
    }

    public void ChangeResolution( int index)
    {
        string[] resolutionStr = resolutionDropdown.options[index].text.Split('x');
        int width = int.Parse(resolutionStr[0]);
        int height= int.Parse(resolutionStr[1]);

        Screen.SetResolution(width, height, Screen.fullScreen);
    }


    public void SetVolume(float volume)

    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

    }
}