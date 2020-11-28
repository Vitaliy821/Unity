using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamage : MonoBehaviour
{
    private Player_controller _player;
    private MovementControl _movementControl;
    

    private void OnTriggerEnter2D(Collider2D info)
    {
        _movementControl = info.GetComponent<MovementControl>();
        _player = info.GetComponent<Player_controller>();
        if (_player != null)
        {
            _movementControl.DoubleDamage(true); 
        }
    }

   
}
