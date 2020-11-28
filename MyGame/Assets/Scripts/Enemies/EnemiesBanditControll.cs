using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesBanditControll : Enemies_ControllerBase
{
    [Header("Strike")]
    [SerializeField] private Transform _strikePoint;
    [SerializeField] private int _damage;
    [SerializeField] private float _strikeRange;
    [SerializeField] private LayerMask _enemies;
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

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_strikePoint.position, new Vector3(_strikeRange, _strikeRange, 0));
    }

    public override void TakeDamage(int damage, DamageType type = DamageType.Casual, Transform player = null)
    {
        
        base.TakeDamage(damage, type, player);
    }

    protected void Strike()
    {

        Collider2D player = Physics2D.OverlapBox(_strikePoint.position, new Vector2(_strikeRange, _strikeRange), 0, _enemies);
        if (player != null)
        {
            Player_controller playerController = player.GetComponent<Player_controller>();
                playerController.TakeDamage(_damage);
        }
    }

    protected override void TryToDamage(Collider2D enemy)
    {
        if (_currentState == EnemyState.Idle || _currentState == EnemyState.Hurt)
            return;

        Player_controller player = enemy.GetComponent<Player_controller>();

        if (player == null)
            return;
       
            player.TakeDamage(_damage, DamageType.Casual, transform);
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
            ChangeState(EnemyState.Strike);
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
            case EnemyState.Strike:
                _attacking = true;
                break;
        }
    }

    protected override void EndState()
    {
        switch (_currentState)
        {
            case EnemyState.Strike:
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
        _enemyAnimator.SetBool(EnemyState.Strike.ToString(), false);
        _enemyAnimator.SetBool(EnemyState.Hurt.ToString(), false);
    }

    protected virtual void DoStateAction()
    {

        switch (_currentState)
        {
            case EnemyState.Strike:
                Strike();
                break;
        }
    }

    public override void GetHurt()
    { 
        base.GetHurt();
    }
}
