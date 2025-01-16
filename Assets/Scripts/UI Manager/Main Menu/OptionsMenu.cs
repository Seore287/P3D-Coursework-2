using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle fullscreenToggle, vsyncToggle;

    [Header("Audio Options")]
    public AudioMixer gameAudioMixer;
    public TMP_Text masterLabel, musicLabel, sfxLabel;
    public Slider masterSlider, musicSlider, sfxSlider;

    [Header("Resolution Options")]
    public TMP_Text resolutionLabel;
    public List<ResolutionItem> resolutions = new List<ResolutionItem>();
    private int selectedResolution;
    
    void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0 )
        {
            vsyncToggle.isOn = false;
        } else
        {
            vsyncToggle.isOn = true;
        }

        bool resolutionFound = false;
        for ( int i = 0; i < resolutions.Count; i++ )
        {
            if (Screen.width == resolutions[i].hVal && Screen.height == resolutions[i].vVal)
            {
                resolutionFound = true;
                selectedResolution = i;

                UpdateResolutionLabel();
            }
        }

        if (!resolutionFound)
        {
            ResolutionItem newResolution = new ResolutionItem();
            newResolution.hVal = Screen.height;
            newResolution.vVal = Screen.width;

            resolutions.Add(newResolution);
            selectedResolution = resolutions.Count - 1;

            UpdateResolutionLabel();
        }

        float volume = 0f;

        gameAudioMixer.GetFloat("MasterVol", out volume);
        masterSlider.value = volume;

        gameAudioMixer.GetFloat("MusicVol", out volume);
        musicSlider.value = volume;

        gameAudioMixer.GetFloat("SFXVol", out volume);
        sfxSlider.value = volume;

        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
    }

    public void ResolutionLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        UpdateResolutionLabel();
    }

    public void ResolutionRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }
        UpdateResolutionLabel();
    }

    public void UpdateResolutionLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].hVal.ToString() + 
            "x" + resolutions[selectedResolution].vVal.ToString();
    }

    public void ApplyGraphics()
    {
        if (vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        } else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].hVal, 
            resolutions[selectedResolution].vVal, fullscreenToggle.isOn);
    }

    public void SetMasterVolume()
    {
        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        gameAudioMixer.SetFloat("MasterVol", masterSlider.value);

        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
    }

    public void SetMusicVolume()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        gameAudioMixer.SetFloat("MusicVol", musicSlider.value);

        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }

    public void SetSFXVolume()
    {
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
        gameAudioMixer.SetFloat("SFXVol", sfxSlider.value);

        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
    }

    [System.Serializable]
    public class ResolutionItem
    {
        public int hVal, vVal;
    }
}
