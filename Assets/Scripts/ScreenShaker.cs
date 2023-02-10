using System;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField] private Transform _shakeTarget = null;

    [SerializeField] private float _globalShakeMultiplier = 1f;
    [SerializeField] private float _traumaPower = 2f;
    [SerializeField] private float _traumaDecay = 1f;
    [SerializeField] private float _shakeSpeed = 10f;

    private float _trauma = 0f;

    public void AddTrauma(float trauma)
    {
        _trauma += trauma;
    }

    private void Update()
    {
        _trauma = Mathf.Max(0, _trauma - _traumaDecay * Time.deltaTime);
        ApplyTrauma();
    }

    private void ApplyTrauma()
    {
        Vector2 offset = new Vector2(
            Mathf.PerlinNoise(Time.time * _shakeSpeed, 0.5f),
            Mathf.PerlinNoise(0.5f, Time.time * _shakeSpeed)
        );
        _shakeTarget.localPosition = (offset * 2 - Vector2.one) * _globalShakeMultiplier * Mathf.Pow(_trauma, _traumaPower);
    }
}