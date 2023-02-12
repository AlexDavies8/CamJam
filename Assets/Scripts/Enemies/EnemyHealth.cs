using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private string _hurtSound = "EnemyHurt";
    [SerializeField] private string _deathSound = "EnemyDeath";
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
                GameManager.Instance.GetGlobalComponent<AudioManager>().PlaySound(_deathSound);
                _animator.Play("Death", 0);
                Destroyed = true;
            }
            else
            {
                GameManager.Instance.GetGlobalComponent<AudioManager>().PlaySound(_hurtSound);
                //_animator.Play("Damage", 0);
            }
        }
    }
}