using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class ItemBaseController : MonoBehaviour, Interactable
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text message;

    public void Interact()
    {
        Time.timeScale = 0;
        message.text = "Item";
        var text = button.GetComponent<TMP_Text>()?.text;
        text = "Take";
        button.onClick.AddListener(() => { 
            canvas.gameObject.SetActive(false);
            Time.timeScale = 1;
            Destroy(gameObject);
        });
        canvas.gameObject.SetActive(true);
    }
}
