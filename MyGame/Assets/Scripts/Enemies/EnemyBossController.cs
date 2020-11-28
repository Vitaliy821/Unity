using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBossController : EnemyWizardController
{
    ServiceManager serviceManager;

    [Header("Strike")]
    [SerializeField] private Transform _strikePoint;
    [SerializeField] private int _damage;
    [SerializeField] private float _strikeRange;
    [SerializeField] private LayerMask _enemies;

    [Header("PowerStrike")]
    [SerializeField] private Collider2D _strikeCollider;
    [SerializeField] private int _powerStrikeDamage;
    [SerializeField] private float _powerStrikeSpeed;
    [SerializeField] private float _powerStrikeRange;


    [SerializeField] private float _waitTime;
    private float _currentStrikeRange;
    private bool _fightStarted;
    private EnemyState _stateOnHold;
    private bool _inRage;

    private EnemyState[] _attackStates = { EnemyState.Strike, EnemyState.PowerStrike, EnemyState.Shoot };

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(_currentState == EnemyState.Move && _attacking)
        {
            TurnToPlayer();
            if (CanAttack())
                ChangeState(_stateOnHold);
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_strikePoint.position, new Vector3(_strikeRange, _strikeRange, 0));
    }

    public override void TakeDamage(int damage, DamageType type = DamageType.Casual, Transform player = null)
    {
        if (_currentState == EnemyState.PowerStrike && type != DamageType.Projectile || _currentState == EnemyState.Hurt)
            return;
        base.TakeDamage(damage, type, player); 

        if(_currentHp <= _maxHP / 2 && !_inRage)
        {
            _inRage = true;
            ChangeState(EnemyState.Hurt);
        }
    }

    protected void Strike()
    {

        Collider2D player = Physics2D.OverlapBox(_strikePoint.position, new Vector2(_strikeRange, _strikeRange), 0, _enemies);
        if (player != null)
        {
            Player_controller playerController = player.GetComponent<Player_controller>();
            int damage = _inRage ? _damage * 2 : _damage;
            if (playerController != null)
                playerController.TakeDamage(damage);
        }
    }

    protected void StrikeWithPower()
    {
        _strikeCollider.enabled = true;
        _enemyRB.velocity = transform.right * _powerStrikeSpeed;

    }

    protected void EndPowerStrike()
    {
        _strikeCollider.enabled = false;
        _enemyRB.velocity = Vector2.zero;
    }

    protected override void TryToDamage(Collider2D enemy)
    {
        if (_currentState == EnemyState.Idle || _currentState == EnemyState.Shoot || _currentState == EnemyState.Hurt)
            return;

        Player_controller player = enemy.GetComponent<Player_controller>();

        if (player == null)
            return;
        if (_inRage)
        {
            player.TakeDamage(_powerStrikeDamage*2, DamageType.PowerStrike, transform);
        }
        else
        {
            player.TakeDamage(_powerStrikeDamage, DamageType.PowerStrike, transform);
        }
    }

    protected override void CheckPlayerInRange()
    {
        if (_player == null)
            return;
        if (Vector2.Distance(transform.position, _player.transform.position) < _angerRange)
        {
            _isAngry = true;
            if (!_fightStarted)
            {   
                StopCoroutine(ScanForPlayer());
                StartCoroutine(BeginNewCircle()); 
            }

        }
        else
        {
            _isAngry = false;
        }
    }

    protected override void ChangeState(EnemyState state)
    {
        if (_currentState == state)
            return;

        if (state == EnemyState.PowerStrike || state == EnemyState.Strike)
        {
            _attacking = true;
            _currentStrikeRange = state == EnemyState.Strike ? _strikeRange : _powerStrikeRange;
            _enemyRB.velocity = Vector2.zero;

            if (!CanAttack())
            {
                _stateOnHold = state;
                state = EnemyState.Move;
            }
        }
        base.ChangeState(state);

    }

    private bool CanAttack()
    {
        return Vector2.Distance(transform.position, _player.transform.position) < _currentStrikeRange;

    }

    protected override void DoStateAction()
    {
        base.DoStateAction();
        switch (_currentState)
        {
            case EnemyState.Strike:
                Strike();
                break;
            case EnemyState.PowerStrike:
                StrikeWithPower();
                break;
        }
    }

    protected override void EndState()
    {
        switch (_currentState)
        {
            case EnemyState.PowerStrike:
                EndPowerStrike();
                _attacking = false;
                break;
            case EnemyState.Strike:
                _attacking = false;
                break;
            case EnemyState.Hurt:
                _enemyAnimator.SetBool("Rage", true);
                _fightStarted = false;
                break;   
        }
        base.EndState();
        if (_currentState == EnemyState.PowerStrike || _currentState == EnemyState.Shoot || _currentState == EnemyState.Strike || _currentState == EnemyState.Hurt)
           StartCoroutine(BeginNewCircle());
    }


    protected override void ResetState()
    {
        base.ResetState();
        _enemyAnimator.SetBool(EnemyState.Strike.ToString(), false);
        _enemyAnimator.SetBool(EnemyState.PowerStrike.ToString(), false);
    }
    private IEnumerator BeginNewCircle()
    {
        if (_fightStarted)
        {
            ChangeState(EnemyState.Idle);
            CheckPlayerInRange();
            if (!_isAngry)
            {
                _fightStarted = false;
                StartCoroutine(ScanForPlayer());
                yield break;
            }
            yield return new WaitForSeconds(_waitTime);
        }
        _fightStarted = true;
        TurnToPlayer();
        ChooseNextAttackScate();
    }

    private void ChooseNextAttackScate()
    {
        int state = Random.Range(0, _attackStates.Length);
        ChangeState(_attackStates[state]);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        serviceManager.ChangeLvl(0);

    }
   

}
