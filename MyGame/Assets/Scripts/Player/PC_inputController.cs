using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MovementControl))]
public class PC_inputController : MonoBehaviour
{
    MovementControl _playerMovement;
    DateTime _strikeClickTime; 
    float _move;
    bool _jump;
    bool _crawling;
    bool _canAttack;


    void Start()
    {
        _playerMovement = GetComponent<MovementControl>();
    }

    // Start is called before the first frame update
    void Update()
    {
        _move = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonUp("Jump"))
        { 
            _jump = true;
        }

        _crawling = Input.GetKey(KeyCode.C);
        if (!isPointerOverUI())
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _strikeClickTime = DateTime.Now;
                _canAttack = true;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                float holdtime = (float)(DateTime.Now - _strikeClickTime).TotalSeconds;
                if (_canAttack)
                    _playerMovement.StartStrike(holdtime);
                _canAttack = false;
            }
        }

        if((DateTime.Now - _strikeClickTime).TotalSeconds >= _playerMovement.ChargeTime * 2 && _canAttack)
        {
           
            _playerMovement.StartStrike(_playerMovement.ChargeTime);
            _canAttack = false;
        }

        if (Input.GetMouseButton(1))
        {
            _playerMovement.Block();
        }

       
        
    }

    void FixedUpdate()
    {
        _playerMovement.Move(_move, _jump, _crawling);
        _jump = false;
    }

    private bool isPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
}
