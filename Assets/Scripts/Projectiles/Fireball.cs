using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float damage = 10f;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _initialVelocity = 15f;
    [SerializeField] private float _destroyDelay = 5f;
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(Vector2.up * _initialVelocity, ForceMode2D.Impulse);
        _animator.SetFloat("Velocity", _initialVelocity);
        
        GameObject.Destroy(gameObject, _destroyDelay);
    }

    private void FixedUpdate()
    {
        transform.up = _rb.velocity.normalized;
        _animator.SetFloat("Velocity", _rb.velocity.y);
    }
}
