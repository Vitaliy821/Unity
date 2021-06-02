using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PauseButtonController : MonoBehaviour
{
    [SerializeField] InGameMenuController controller;
    Button button;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            controller.ActivateMenu();
        });
    }
}
