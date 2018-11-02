using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Dropdown sizeDropdown;
    public Dropdown qualityDropdown;

    public Text masterVolumeText;
    public Slider masterVolume;
    public Text sfxVolumeText;
    public Slider sfxVolume;
    public Text musicVolumeText;
    public Slider musicVolume;

    public Text mouseVerticalText;
    public Slider mouseVertical;
    public Text mouseHorizontalText;
    public Slider mouseHorizontal;
    /*
    public Text xSliderText;
    public Slider xSlider;
    public Text ySliderText;
    public Slider ySlider;
    public Text zSliderText;
    public Slider zSlider;
    */
    public Data data;

    Resolution[] resolutions;

    void Start()
    {
        data.Load();

        PopulateRes();

        if (data.isFirstRun)
        {
            FirstRun();
        }
        else
        {
            Run();
        }

        sizeDropdown.RefreshShownValue();
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.RefreshShownValue();

        data.Save();
    }

    public void FirstRun()
    {
        Key.Populate();
        SetFullscreen(0);
        sizeDropdown.value = 0;
        SetResolution(resolutions.Length - 1);
        resolutionDropdown.value = 0;
        SetQuality(3);
        qualityDropdown.value = 3;

        SetMasterVolume(0);
        masterVolume.value = 0;
        SetSFXVolume(0);
        sfxVolume.value = 0;
        SetMusicVolume(0);
        musicVolume.value = 0;

        SetMouseVertical(0);
        mouseVertical.value = 0;
        SetMouseHorizontal(0);
        mouseHorizontal.value = 0;

        /*
        SetXSize(10);
        xSlider.value = 10;
        SetZSize(10);
        ySlider.value = 10;
        SetDoors(0);
        zSlider.value = 0;
        */

        data.isFirstRun = false;
    }

    public void Run()
    {
        Key.Load();
        SetFullscreen(data.screenIndex);
        sizeDropdown.value = data.screenIndex;
        SetResolution(data.resolutionIndex);
        resolutionDropdown.value = data.resolutionIndex;
        SetQuality(data.qualityIndex);
        qualityDropdown.value = data.qualityIndex;

        SetMasterVolume(data.masterVolume);
        masterVolume.value = data.masterVolume;
        SetSFXVolume(data.sfxVolume);
        sfxVolume.value = data.sfxVolume;
        SetMusicVolume(data.musicVolume);
        musicVolume.value = data.musicVolume;

        SetMouseVertical(Data.mouseVertical);
        mouseVertical.value = Data.mouseVertical;
        SetMouseHorizontal(Data.mouseHorizontal);
        mouseHorizontal.value = Data.mouseHorizontal;

        /*
        SetXSize(Data.xSize);
        xSlider.value = Data.xSize;
        SetZSize(Data.zSize);
        ySlider.value = Data.zSize;
        SetDoors(Data.Doors);
        zSlider.value = Data.Doors;
        */
    }

    public void PopulateRes()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (Resolution res in resolutions)
        {
            string option = res.width + " x " + res.height + " @ " + res.refreshRate;
            options.Add(option);
        }
        resolutionDropdown.AddOptions(options);
    }

    public void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
        data.resolutionIndex = index;
        data.Save();
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
        masterVolumeText.text = volume.ToString() + " dB"; 
        data.masterVolume = volume;
        data.Save();
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
        sfxVolumeText.text = volume.ToString() + " dB";
        data.sfxVolume = volume;
        data.Save();
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
        musicVolumeText.text = volume.ToString() + " dB";
        data.musicVolume = volume;
        data.Save();
    }

    public void SetMouseVertical(float value)
    {
        mouseVerticalText.text = value.ToString();
        Data.mouseVertical = value;
        data.Save();
    }
    public void SetMouseHorizontal(float value)
    {
        mouseHorizontalText.text = value.ToString();
        Data.mouseHorizontal = value;
        data.Save();
    }
    /*
    public void SetXSize(float value)
    {
        xSliderText.text = value.ToString();
        Data.xSize = (int)value;
        data.Save();
    }
    public void SetZSize(float value)
    {
        ySliderText.text = value.ToString();
        Data.zSize = (int)value;
        data.Save();
    }
    public void SetDoors(float value)
    {
        zSliderText.text = value.ToString();
        Data.Doors = value;
        data.Save();
    }
    */


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        data.qualityIndex = qualityIndex;
        data.Save();
    }

    public void SetFullscreen(int index)
    {
        if (index == 0)
        {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
            sizeDropdown.value = 0;
        }
        if (index == 1)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            sizeDropdown.value = 1;
        }
        data.screenIndex = index;
        data.Save();
    }

    public void Close()
    {
        Application.Quit();
    }
}
