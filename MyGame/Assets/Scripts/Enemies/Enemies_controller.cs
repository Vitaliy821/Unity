using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_controller : MonoBehaviour
{
    [SerializeField] private int _hp;

    public void TakeDamage(int damage, DamageType powerStrike)
    {
        _hp -= damage;
        if (_hp < 0)
            OnDeath();

        Debug.Log("damage = " + damage);
        Debug.Log("Hp = " + _hp);
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
