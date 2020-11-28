using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjecttitleController : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D info)
    {
        Enemies_controller enemy = info.GetComponent<Enemies_controller>();
        if (enemy != null)
            enemy.TakeDamage(_damage, DamageType.Projectile);
        Destroy(gameObject);
    }
}
