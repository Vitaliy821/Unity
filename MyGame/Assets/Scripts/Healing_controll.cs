using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing_controll : MonoBehaviour
{
    [SerializeField] private int _healing;
    private Player_controller _player;
    private void OnTriggerEnter2D(Collider2D info)
    {
        _player = info.GetComponent<Player_controller>();
        if (_player != null)
        {
            _player.RestoreHP(_healing);
        }
    }

   
}
