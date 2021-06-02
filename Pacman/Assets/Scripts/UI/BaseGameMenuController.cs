using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameMenuController : MonoBehaviour
{
    protected ServiceManager _serviceManager;

    [SerializeField] protected GameObject _menu;


    [Header ("Main Buttons")]
    [SerializeField] protected Button _play;
    [SerializeField] protected Button _quit;
    protected virtual void Start()
    {
        _serviceManager = ServiceManager.Instance;
        _quit.onClick.AddListener(OnQuitClicked);
  
    }

    protected virtual void OnDestroy()
    { 
        _quit.onClick.RemoveListener(OnQuitClicked);
    }
    
    protected void Update(){ }

    protected virtual void OnMenuClicked()
    {   
        _menu.SetActive(!_menu.activeInHierarchy);
    }

    private void OnQuitClicked()
    {
        _serviceManager.Quit();
    }
}
    
