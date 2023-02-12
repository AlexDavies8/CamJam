using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float _health = 10f;
    public bool Destroyed = false;
    [SerializeField] private Animator _animator;

    private void OnTriggerEnter2D(Collider2D col)
    {
        var fireball = col.GetComponent<Fireball>();
        if (fireball != null)
        {
            _health -= fireball.damage;
            if (_health <= 0)
            {
                _animator.Play("Death", 0);
                Destroyed = true;
            }
            else
            {
                //_animator.Play("Damage", 0);
            }
        }
    }
}