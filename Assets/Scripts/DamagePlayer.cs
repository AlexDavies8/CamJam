using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(_damage);
        }
    }
}
