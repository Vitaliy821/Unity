using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_controller : MonoBehaviour
{
    private ServiceManager _serviceManager;
    

    [SerializeField] private int _maxHp;
    public int _currentHp;
    [SerializeField] private int _maxEnergy;
    [SerializeField] private int _regenEnergy;
    public int _currentEnergy;


    [SerializeField] Slider _hpBar;
    [SerializeField] Slider _energyBar;
    MovementControl _playerMovement;
    private bool _canBeDamaged = true;

    [Header("Block")]
    [SerializeField] private int block;
    [HideInInspector] public bool _isBlocked;
   
  



    void Start()
    {
        _playerMovement = GetComponent<MovementControl>();
        _playerMovement.OnGetHurt += OnGetHurt;
        _currentHp = _maxHp;
        _currentEnergy = _maxEnergy;
        _hpBar.maxValue = _maxHp;
        _hpBar.value = _maxHp;
        _energyBar.maxValue = _maxEnergy;
        _energyBar.value = _maxEnergy;
        _serviceManager = ServiceManager.Instance;
        StartCoroutine(RegenEnergy());
    }


    protected IEnumerator RegenEnergy()
    {
        while (true)
        {
            ChangeEnergy(_regenEnergy);
            yield return new WaitForSeconds(1f);
        }
    }

    public void TakeDamage(int damage, DamageType type = DamageType.Casual, Transform enemy = null)
    {
        
        if (!_canBeDamaged)
            return;
        if (_isBlocked && type == DamageType.Casual || type == DamageType.PowerStrike || type == DamageType.Projectile)
        {
            damage = damage / block;
            Debug.Log(damage);
            _currentHp -= damage;
        }
        else
        {
            _currentHp -= damage;
        }
        _hpBar.value = _currentHp;

        if (_currentHp <= 0)
        {
            onDeath();
        }

        switch (type)
        {
            case DamageType.PowerStrike:
                _playerMovement.GetHurt(enemy.position);
                break;
        }

    }

    private void OnGetHurt(bool canBeDamaged)
    {
        _canBeDamaged = canBeDamaged;
    }

    public void RestoreHP(int hp)
    {
        _currentHp += hp;

        if (_currentHp > _maxHp)
        {
            _currentHp = _maxHp;
        }
        _hpBar.value = _currentHp;
    }

    public bool ChangeEnergy(int value)
    {
        Debug.Log("Max value = "+ value) ;
        if (value < 0 && _currentEnergy < Mathf.Abs(value))
            return false;
        
        _currentEnergy += value;
        if (_currentEnergy > _maxEnergy)
            _currentEnergy = _maxEnergy;

        _energyBar.value = _currentEnergy;
        return true;
    }

    public void onDeath()
    {
        _serviceManager.Restart();
    } 
}
