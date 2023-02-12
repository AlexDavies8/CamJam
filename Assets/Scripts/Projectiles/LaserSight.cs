using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    [SerializeField] private LayerMask _hitLayer;
    [SerializeField] private LineRenderer _line;

    private void Awake()
    {
        _line.positionCount = 2;
        _line.useWorldSpace = true;
    }

    private void FixedUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, transform.up, 100f, _hitLayer.value);
        if (hit)
        {
            _line.SetPosition(0, transform.position);
            _line.SetPosition(1, hit.point);
        }
    }
}
