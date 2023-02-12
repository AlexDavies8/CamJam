using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _initialVelocity = 10f;
    [SerializeField] private Vector2 _relativeDirection = Vector2.up;
    [SerializeField] private float _destroyDelay = 5f;
    [SerializeField] private LayerMask _groundLayer;

    private void Awake()
    {
        var rb = GetComponent<Rigidbody2D>();
        var dir = _relativeDirection.normalized;
        rb.AddForce((transform.up * dir.y + transform.right * dir.x) * _initialVelocity, ForceMode2D.Impulse);
        Destroy(gameObject, _destroyDelay);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((_groundLayer.value & (1 << col.gameObject.layer)) > 0)
        {
            Destroy(gameObject);
        }
        else
        {
            var playerController = col.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Hit Player
                playerController.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
