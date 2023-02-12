using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerMotor _playerMotor;
    [SerializeField] private float _offset = 2f;
    [SerializeField] private float _smoothFactor = 1f;

    private float _targetPosition;

    private void Awake()
    {
        _targetPosition = _playerMotor.transform.position.y;
    }

    private void FixedUpdate()
    {
        if (_playerMotor.OnGround && _playerMotor.Velocity.y <= 0f)
            _targetPosition = _playerMotor.transform.position.y;
        float newY = Mathf.MoveTowards(transform.position.y, _targetPosition + _offset,
            _smoothFactor * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
