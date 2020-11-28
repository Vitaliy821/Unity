using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [Header ("Volume")]
    [SerializeField] private Slider _volume;
    [SerializeField] private AudioMixer _masterMixer;


    [Header("Full screen")]
    [SerializeField] private Toggle _fullScreen;



    [SerializeField] private TMP_Dropdown _resolutionDropDown;
    private Resolution[] _avaliableResolutions;


    [SerializeField] private TMP_Dropdown _qualityDropDown;
    private string[] _qualityLevels;

   
    void Start()
    {
        _volume.onValueChanged.AddListener(OnVolumeChanged);

        _fullScreen.onValueChanged.AddListener(OnFullScreenChenged);

        _resolutionDropDown.onValueChanged.AddListener(OnResolutionChanged); 

        _qualityDropDown.onValueChanged.AddListener(OnQualityChanged);

        _avaliableResolutions = Screen.resolutions;
        _resolutionDropDown.ClearOptions();

        int currentIndex = 0;
        List<string> options = new List<string>();
        for(int i =0; i<_avaliableResolutions.Length; i++)
        {
            if (_avaliableResolutions[i].width <= 800)
                continue;
            options.Add(_avaliableResolutions[i].width + "x" + _avaliableResolutions[i].height);
            if (_avaliableResolutions[i].width == Screen.currentResolution.width && _avaliableResolutions[i].height == Screen.currentResolution.height)
                currentIndex = i;
        }
        _resolutionDropDown.AddOptions(options);
        _resolutionDropDown.value = currentIndex;
        _resolutionDropDown.RefreshShownValue();

        _qualityLevels = QualitySettings.names;
        _qualityDropDown.ClearOptions();    
        _qualityDropDown.AddOptions(_qualityLevels.ToList());
        int quilityLvl = QualitySettings.GetQualityLevel();
        _qualityDropDown.value = quilityLvl;
        _qualityDropDown.RefreshShownValue();
    }

    private void OnDestroy()
    {
        _volume.onValueChanged.RemoveListener(OnVolumeChanged);
        _fullScreen.onValueChanged.RemoveListener(OnFullScreenChenged);
        _resolutionDropDown.onValueChanged.RemoveListener(OnResolutionChanged);
        _qualityDropDown.onValueChanged.RemoveListener(OnQualityChanged);

    }


    private void OnVolumeChanged(float volume)
    {
        _masterMixer.SetFloat("Volume", volume);
    }

    private void OnFullScreenChenged(bool value)
    {
        Screen.fullScreen = value;
    }

    private void OnResolutionChanged(int resolutionindex)
    {
        Resolution resolution = _avaliableResolutions[resolutionindex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void OnQualityChanged(int qualitylvl)
    {
        QualitySettings.SetQualityLevel(qualitylvl, true);
    }
}
 