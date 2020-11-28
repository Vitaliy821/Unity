﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWizardController : Enemies_ControllerBase
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _arrowSpeed;
    [SerializeField] protected float _angerRange;

    protected bool _isAngry;
    protected bool _attacking;


    protected override void Start()
    {
        base.Start();
        _player = FindObjectOfType<Player_controller>();
        StartCoroutine(ScanForPlayer());
    }

    protected override void Update()
    {
        if (_isAngry)
            return;
        base.Update();
    }
    private void Shoot()
    {
        GameObject arrow = Instantiate(_projectilePrefab, _shootPoint.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = transform.right * _arrowSpeed;
        arrow.GetComponent<SpriteRenderer>().flipX = !_faceRight;
        Destroy(arrow, 2f);
    }

    protected IEnumerator ScanForPlayer()
    {
        while (true)
        {
            CheckPlayerInRange();
            yield return new WaitForSeconds(1f);
        }
    }

    protected virtual void CheckPlayerInRange()
    {
        if (_player == null || _attacking)
            return;
        if (Vector2.Distance(transform.position, _player.transform.position) < _angerRange)
        {
            _isAngry = true;

            TurnToPlayer();
            ChangeState(EnemyState.Shoot);
        }
        else
        {
            _isAngry = false;
        }
    }

   

    protected override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);
        switch (state)
        {
            case EnemyState.Shoot:
                _attacking = true;
                _enemyRB.velocity = Vector2.zero;
                break;
        }
    }

    protected override void EndState()
    {
        switch (_currentState)
        {
            case EnemyState.Shoot:
                _attacking = false;
                break;
        }

        if (!_isAngry)
            ChangeState(EnemyState.Idle);

        base.EndState();
    }

    protected override void ResetState()
    {
        base.ResetState();
        _enemyAnimator.SetBool(EnemyState.Shoot.ToString(), false);
        _enemyAnimator.SetBool(EnemyState.Hurt.ToString(), false);
    }

    protected virtual void DoStateAction()
    {
 
        switch (_currentState)
        {
            case EnemyState.Shoot:
                Shoot();
                break;
        }
    }

    public override void GetHurt()
    {
        _attacking = false;
        base.GetHurt();
    }

}
