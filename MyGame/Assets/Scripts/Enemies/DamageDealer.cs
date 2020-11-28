using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeReference] private float _timeDelay;
    private Player_controller _player;
    private DateTime _lastncounter;
    private MovementControl _playerControl;
    private void OnTriggerEnter2D(Collider2D info)
    {
        if ((DateTime.Now - _lastncounter).TotalSeconds < 0.1f)
            return;
        _lastncounter = DateTime.Now; 
        _player = info.GetComponent<Player_controller>();
        if (_player != null)
        {
            _player.TakeDamage(_damage);
        }
        
    }

    private void OnTriggerExit2D(Collider2D info)
    {
        if (_player == info.GetComponent<Player_controller>())
            _player = null;
    }
    private void Update()
    {
            if(_player !=null && (DateTime.Now - _lastncounter).TotalSeconds > _timeDelay)
        {
            _player.TakeDamage(_damage);
            _lastncounter = DateTime.Now; 
        }
    }
}
