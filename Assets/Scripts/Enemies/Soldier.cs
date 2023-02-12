using System;
using System.Collections;
using System.Collections.Generic;
using MultiState;
using Unity.Mathematics;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private Transform _gunTransform;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _fireAnimation;
    [SerializeField] private string _aimAnimation;
    [SerializeField] private float _aimDistance = 14f;
    
    private StateMachine _stateMachine;

    private string _stateName;

    private void Awake()
    {
        _stateMachine = new StateMachine();

        var idleState = new ActionState();
        var aimState = new ActionState()
            .OnEnter(() => PlayAnimation(_aimAnimation))
            .OnTick(AimGun);
        var fireState = new ActionState()
            .OnEnter(() => PlayAnimation(_fireAnimation));
        
        _stateMachine.AddTransition(aimState, idleState, () => GetPlayerDistance() > _aimDistance);
        _stateMachine.AddTransition(idleState, aimState, () => GetPlayerDistance() <= _aimDistance);
        
        _stateMachine.AddTransition(aimState, fireState, () => _stateName == "Fire");
        _stateMachine.AddTransition(fireState, aimState, () =>_stateName == "Aim");
        
        _stateMachine.SetState(idleState);
    }

    private void FixedUpdate()
    {
        _stateMachine.Tick();
    }

    private void AimGun()
    {
        var playerTransform = GameManager.Instance.GetGlobalComponent<PlayerController>().transform;
        
        Vector2 aimDelta = (playerTransform.position - _gunTransform.position).normalized;
        var aimAngle = aimDelta.LookDirection();

        if (aimDelta.x <= 0)
        {
            _gunTransform.rotation = Quaternion.Euler(0f, 0f, 270f + aimAngle);
            _bodyTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            _gunTransform.rotation = Quaternion.Euler(0f, 180f, -aimAngle - 90f);
            _bodyTransform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private void PlayAnimation(string animationName)
    {
        _animator.Play(animationName, 0);
    }

    public void SetState(string stateName)
    {
        _stateName = stateName;
    }

    private float GetPlayerDistance()
    {
        return Mathf.Abs(GameManager.Instance.GetGlobalComponent<PlayerController>().transform.position.y - transform.position.y);
    }
}
