using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public abstract class Enemies_ControllerBase : MonoBehaviour
{
    protected Rigidbody2D _enemyRB;
    protected Animator _enemyAnimator;
    protected Player_controller _player;

    [Header("Canvas")]
    [SerializeField] GameObject _canvas;    


    [Header("HP")]
    [SerializeField] protected int _maxHP;
    [SerializeField] protected Slider _hpBar;
    protected int _currentHp;


    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _range;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private bool _checkRange;
    [SerializeField] private Transform _wallCheck;
    protected bool _faceRight = true;
    protected Vector2 _startPoint;


    [Header("State changes")]
    [SerializeField] private float _maxStateTime;
    [SerializeField] private float _minStateTime;
    [SerializeField] private EnemyState[] _availableStates;
    protected float _lastStateChange;
    protected float _timeToNextChange;
    protected EnemyState _currentState;


    [Header("Collision damage")]
    [SerializeField] protected int _collisionDamage;
    [SerializeField] protected float _collisionTimeDelay;
    [SerializeField] private DamageType _collisionDamageType;

    [Header("Fight Settings")]
    [SerializeField] protected int _hurtDelay;
    
    protected float _lastDamageTime;
   

    protected virtual void Start()
    {

        _startPoint = transform.position;
        _enemyRB = GetComponent<Rigidbody2D>();
        _enemyAnimator = GetComponent<Animator>();
        _currentHp = _maxHP;
        _hpBar.maxValue = _maxHP;
        _hpBar.value = _maxHP;

    }

    protected virtual void FixedUpdate()
    {
        if (_currentState == EnemyState.Death)
            return;
        if (IsGroundEnding() || IsInRange() || Physics2D.OverlapPoint(_wallCheck.position, _whatIsGround))
            Flip();

        if (_currentState == EnemyState.Move)
            Move();
    }

    private bool IsInRange()
    {
        return _checkRange && ((transform.position.x - _startPoint.x < -_range) ||
            (transform.position.x - _startPoint.x > _range));
    }

    protected virtual void Update()
    {
        if (_currentState == EnemyState.Death)
            return;
        if (Time.time - _lastStateChange > _timeToNextChange)
        {
            GetRandomState();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (_currentState == EnemyState.Death)
            return;
        TryToDamage(collision.collider);
    }

    protected virtual void TryToDamage(Collider2D enemy)
    {
        if (Time.time - _lastDamageTime < _collisionTimeDelay)
            return;

        Player_controller player = enemy.GetComponent<Player_controller>();

        if (player != null)
            player.TakeDamage(_collisionDamage , _collisionDamageType, transform);

    }

    protected virtual void Move()
    {
        _enemyRB.velocity = transform.right * new Vector2(_speed, _enemyRB.velocity.y);
    }
    protected virtual void TurnToPlayer()
    {
        if (_player == null)
            return;
        if ((_player.transform.position.x - transform.position.x > 0 && !_faceRight)
            || (_player.transform.position.x - transform.position.x < 0 && _faceRight))
        {
            Flip();
        }
    }

    protected virtual void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
        _canvas.transform.Rotate(0, 180, 0); 
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_range * 2, 0.5f, 0));
    }

    #region PublicMethods

    public virtual void TakeDamage(int damage, DamageType type = DamageType.Casual, Transform player = null) 
    {
        _currentHp -= damage;

        
        if (_currentHp <=0)
        {
            _currentHp = 0;
            _hpBar.value = _currentHp;
            ChangeState(EnemyState.Death);
        }
        _hpBar.value = _currentHp;

    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    #endregion

    private bool IsGroundEnding()
    {
        return !Physics2D.OverlapPoint(_groundCheck.position, _whatIsGround);
    }

    protected void GetRandomState()
    {
        int state = Random.Range(0, _availableStates.Length - 1);

        if (_currentState == EnemyState.Idle && _availableStates[state] == EnemyState.Idle)
        {
            GetRandomState();
        }
        _timeToNextChange = Random.Range(_minStateTime, _maxStateTime);
        ChangeState(_availableStates[state]);

    }


    protected virtual void ChangeState(EnemyState state)
    {

        if (_currentState == EnemyState.Death)
            return;
        ResetState();
        _currentState = EnemyState.Idle;

        if (_currentState != EnemyState.Idle)
            _enemyAnimator.SetBool(_currentState.ToString(), false);

        if (state == EnemyState.Hurt)
            GetHurt();
        else if (state != EnemyState.Idle)
            _enemyAnimator.SetBool(state.ToString(), true);

        _currentState = state;
        _lastStateChange = Time.time;

        switch (_currentState)
        {
            case EnemyState.Idle:
                _enemyRB.velocity = Vector2.zero;
                break;
            case EnemyState.Move:
                _startPoint = transform.position;
                break;
        }
    }

    protected virtual void EndState()
    {
        if (_currentState == EnemyState.Death)
            OnDeath();
        if (_currentState == EnemyState.Hurt)
            EndHurt();

        ResetState();
    }

    protected virtual void ResetState()
    {
        _enemyAnimator.SetBool(EnemyState.Move.ToString(), false);
        _enemyAnimator.SetBool(EnemyState.Death.ToString(), false);
    }

    public virtual void GetHurt()
    {
        _enemyRB.velocity = Vector2.zero;
        _enemyAnimator.SetBool("Hurt", true);
    }

    private void EndHurt()
    {
        _enemyAnimator.SetBool("Hurt", false);
    }

   
}

public enum EnemyState
{
    Idle,
    Move,
    Shoot,
    Strike,
    PowerStrike,
    Hurt,
    Death,
}