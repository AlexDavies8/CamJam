using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    [SerializeField] private float _depth = 0f;
    private Transform _camera;

    private void Awake()
    {
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        
    }
}
