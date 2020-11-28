    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(AudioSource))]
[RequireComponent(typeof(Player_controller))]
public class MovementControl : MonoBehaviour
{
    public event Action<bool> OnGetHurt = delegate { };

    Rigidbody2D _playerRB;
    private Animator _playerAnimator;
    private Player_controller _playerController;

    [Header("horizontal movement")]
    [SerializeField] private float _speed;
    private bool _faceRight = true;
    private bool _canMove = true;

    [Header("Jummping")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _radius;
    [SerializeField] private bool _airControll;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private int _jumpCost;
    bool _grounded;

    [Header("Crawling")]
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private Transform _cellCheck;
    bool _canStand;

    [Header("Strike")]
    [SerializeField] private Transform _strikePoint;
    [SerializeField] private int _damage;
    [SerializeField] private float _strikeRange;
    [SerializeField] private LayerMask _enemies;
    [SerializeField] private int _strikeCost;
    private bool _isStriking;

    [Header("PowerStrike")]
    [SerializeField] private float _chargeTime;
    public float ChargeTime => _chargeTime; 
    [SerializeField] private Transform _powerStrikePoint;
    [SerializeField] private int _powerDamage;
    [SerializeField] private float _powerStrikeRange;
    [SerializeField] private int _powerStrikeCost;

    [SerializeField] private float _pushForce;
    private float _lastHurtTime;

    [Header("Audio")]
    [SerializeField] private InGameSound _runClip;
    private InGameSound _currentSound;
    private AudioSource _audioSource; 




    void Start()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerController = GetComponent<Player_controller>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheck.position, _radius, _whatIsGround);
        if (_playerAnimator.GetBool("Hurt") && _grounded && Time.time > 0.5f)
            EndHurt();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_cellCheck.position, _radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_strikePoint.position, _strikeRange);
    }

    void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }

    public void Move (float move, bool jump, bool crawling)
    {
        if (!_canMove)
            return;
        #region Movement
        if (move != 0 && (_grounded || _airControll))
            _playerRB.velocity = new Vector2(_speed * move, _playerRB.velocity.y);


        if (move > 0 && !_faceRight)
        {
            Flip();
        }
        else if (move < 0 && _faceRight)
        {
            Flip();
        }
        #endregion

        #region Jumping
       

        if (jump && _grounded && _playerController._currentEnergy > _jumpCost)
        {
            _playerRB.AddForce(Vector2.up * _jumpForce);
            _playerController.ChangeEnergy(-_jumpCost);
            jump = false;
        }
        #endregion

        #region Crawling
        _canStand = !Physics2D.OverlapCircle(_cellCheck.position, _radius, _whatIsGround);

        if (crawling)
        {
            _headCollider.enabled = false;
        }
        else if (!crawling && _canStand)
        {
            _headCollider.enabled = true;
        }
        #endregion

        #region Animation
        _playerAnimator.SetFloat("Speed", Mathf.Abs(move));
        _playerAnimator.SetBool("Jump", !_grounded);
        _playerAnimator.SetBool("Crall", !_headCollider.enabled);
        #endregion

        if (_grounded && _playerRB.velocity.x != 0 && !_audioSource.isPlaying)
            PlayAudio(_runClip);
        else if (!_grounded || _playerRB.velocity.x == 0)
            StopAudio(_runClip);
    }

    public void PlayAudio(InGameSound sound)
    {
        if (_currentSound != null && (_currentSound == sound || _currentSound.Priority > sound.Priority))
            return;

        _currentSound = sound;
        _audioSource.clip = _currentSound.AudioClip;
        _audioSource.loop = _currentSound.Loop;
        _audioSource.pitch = _currentSound.Pitch;
        _audioSource.volume = _currentSound.Volume;
        _audioSource.Play();
    }

    public void StopAudio(InGameSound sound)
    {
        if (_currentSound == null || _currentSound != sound)
            return;

        _audioSource.Stop();
        _audioSource.clip = null;
        _currentSound = null;
    }



    public void StartStrike(float holdtime)
        {
        if (_isStriking)
            return;
        if(holdtime >= _chargeTime)
        {
            if (!_playerController.ChangeEnergy(-_powerStrikeCost))
                return;
            _playerAnimator.SetBool("PowerStrike", true); 
            /*_canMove = false;*/
           
        }
        else
        {
            if (!_playerController.ChangeEnergy(-_strikeCost))
                return;
            _playerAnimator.SetBool("Strike", true);
        }
        _isStriking = true;
        }

    private void EndAnimations()
    {
        _playerAnimator.SetBool("Strike", false);
        _playerAnimator.SetBool("PowerStrike", false);
        _playerAnimator.SetBool("Shoot", false);
    }

    public void GetHurt(Vector2 position)
    {
        _lastHurtTime = Time.time;
        _canMove = false;
        OnGetHurt(false);
        Vector2 pushDirection = new Vector2();
        pushDirection.x = position.x > transform.position.x ? -1 : 1;
        pushDirection.y = 10;
        _playerAnimator.SetBool("Hurt", true);
        EndAnimations();
        _playerRB.AddForce(pushDirection * _pushForce, ForceMode2D.Impulse);
        _isStriking = false;
        
    }

    private void EndHurt()
    {
        _canMove = true;
        _playerAnimator.SetBool("Hurt", false);
        OnGetHurt(true);
    }

    private void ResetPlayer()
    {
        _playerAnimator.SetBool("Strike", false);
        _playerAnimator.SetBool("PowerStrike", false);
        _playerAnimator.SetBool("Hurt", false);

        _isStriking = false;
        _canMove = false;  
    }

        

        public void Strike()
        { 
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_strikePoint.position, _strikeRange, _enemies);

            if (enemies != null)
                foreach (var enemy in enemies)
                    enemy.GetComponent<Enemies_ControllerBase>().TakeDamage(_damage, DamageType.Casual);
        
        }
        public void EndStrike()
        {
        _playerAnimator.SetBool("Strike", false);
        _isStriking = false;
        }

        public void PowerStrike()
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(_powerStrikePoint.position, _powerStrikeRange, _enemies);
            for (int i = 0; i < enemies.Length; i++)
            {
                Enemies_ControllerBase enemy = enemies[i].GetComponent<Enemies_ControllerBase>();
                enemy.TakeDamage(_powerDamage, DamageType.PowerStrike);
            }
        }

        public void EndPowerStrike()
        {
        _playerAnimator.SetBool("PowerStrike", false);
        /*_canMove = true;*/
        _isStriking = false;
        }

    public void Block()
    {
        _playerController._isBlocked = true;
        _playerAnimator.SetBool("Block", true);
        _canMove = false;
    }

    public void EndBlock()
    {
        _playerAnimator.SetBool("Block", false);
        _canMove = true;
    }

    public void DoubleDamage(bool value)
    {
        if (value) {
            _damage *= 2;
                }
        else
            _damage /= 2;
    }

    }