using System;
using MultiState;
using UnityEngine;

public class TimerState : IState
{
    private readonly float _time;
    private Action _onEnter;

    public TimerState(float time, Action onEnter = null)
    {
        _time = time;
        _onEnter = onEnter;
    }

    public bool Completed => _timer < 0f;

    private float _timer;
    
    public void OnEnter()
    {
        _onEnter?.Invoke();
        _timer = _time;
    }

    public void Tick()
    {
        _timer -= Time.deltaTime;
    }

    public void OnExit()
    {
    }
}