using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuController : BaseGameMenuController
{
   
    [SerializeField] private Button _restart;
    [SerializeField] private Button _backToMenu;
   
    protected override void Start()
    {
        base.Start();
        _play.onClick.AddListener(OnMenuClicked);    
        _restart.onClick.AddListener(OnRestartClicked);
        _backToMenu.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnDestroy()   
    {
        _play.onClick.RemoveListener(OnMenuClicked);
        _restart.onClick.RemoveListener(OnRestartClicked);
        _backToMenu.onClick.RemoveListener(OnMainMenuClicked);
    }

    protected override void OnMenuClicked()
    {
        base.OnMenuClicked();
        Time.timeScale = _menu.activeInHierarchy ? 0 : 1;
        _audioManager.Play(UIClipNames.Play);
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            OnMenuClicked();  
    }

    

    public void OnMainMenuClicked()
    {
        ServiceManager.Instance.ChangeLvl((int)Scenes.MainMenu);
        _audioManager.Play(UIClipNames.Menu);
    }

    public void OnRestartClicked()
    {
        ServiceManager.Instance.Restart();
        _audioManager.Play(UIClipNames.Restart);
    }
}
