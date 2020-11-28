using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameMenuController : MonoBehaviour
{
    protected ServiceManager _serviceManager;
    protected UIAudioManager _audioManager;

    [SerializeField] protected GameObject _menu;

    [Header ("Settings Menu")]
    [SerializeField] protected GameObject _settigsMenu;
    [SerializeField] protected Button _closeSettings;

    [Header ("Main Buttons")]
    [SerializeField] protected Button _play;
    [SerializeField] protected Button _settings;
    [SerializeField] protected Button _quit;
    protected virtual void Start()
    {
        _serviceManager = ServiceManager.Instance;
        _audioManager = UIAudioManager.Instance;
        _quit.onClick.AddListener(OnQuitClicked);
        _settings.onClick.AddListener(OnSettingsClicked);
        _closeSettings.onClick.AddListener(OnSettingsClicked);
      
    }

    protected virtual void OnDestroy()
    { 
        _quit.onClick.RemoveListener(OnQuitClicked);
        _settings.onClick.RemoveListener(OnSettingsClicked);
        _closeSettings.onClick.RemoveListener(OnSettingsClicked);
        _closeSettings.onClick.RemoveListener(OnSettingsClicked);
    }
    
    protected void Update(){ }

    protected virtual void OnMenuClicked()
    {   
        _menu.SetActive(!_menu.activeInHierarchy);
        _audioManager.Play(UIClipNames.Menu);
    }

    private void OnQuitClicked()
    {
        _serviceManager.Quit();
        _audioManager.Play(UIClipNames.Quit);
    }

    private void OnSettingsClicked()
    {
        _audioManager.Play(UIClipNames.Settings);
        _settigsMenu.SetActive(!_settigsMenu.activeInHierarchy);
    }

}
    
