using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    [SerializeField] private float _parallaxFactor = 1f;
    [SerializeField] private Vector2 _parallaxDirection = Vector2.up;
    private Transform _camera;

    private Vector2 _centre;
    
    private void Awake()
    {
        _camera = Camera.main.transform;
        _centre = transform.position;
    }

    private void Update()
    {
        Vector2 offset = (Vector2)_camera.position - _centre;
        transform.position = _centre + offset * _parallaxDirection * _parallaxFactor;
    }
}
