using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MultiState;
using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    public UnityEvent OnDie;
    public List<PowerCore> cores = new();
    [SerializeField] private Animator _animator;

    private StateMachine _stateMachine;
    private int _rand;

    private void Awake()
    {
        BuildStateMachine();
    }

    private void BuildStateMachine()
    {
        _stateMachine = new StateMachine();

        var laserBarrage = new AnimationState(_animator, "LaserBarrage");
        var laserSweep = new AnimationState(_animator, "LaserSweep");
        var drillSlam = new AnimationState(_animator, "DrillSlam");
        var drillSweep = new AnimationState(_animator, "DrillSweep");
        var idle = new TimerState(3f, () => _animator.Play("Idle", 0));
        var death = new AnimationState(_animator, "Death");
        
        //stateMachine.AddTransition(idle, laserBarrage, () => idle.Completed && _rand % 4 == 0);
        _stateMachine.AddTransition(idle, laserSweep, () => idle.Completed && _rand % 4 == 1);
        _stateMachine.AddTransition(idle, drillSlam, () => idle.Completed && _rand % 4 == 2);
        //_stateMachine.AddTransition(idle, drillSweep, () => idle.Completed && _rand % 4 == 3);

        _stateMachine.AddTransition(laserBarrage, idle, () => laserBarrage.Completed);
        _stateMachine.AddTransition(laserSweep, idle, () => laserSweep.Completed);
        _stateMachine.AddTransition(drillSlam, idle, () => drillSlam.Completed);
        _stateMachine.AddTransition(drillSweep, idle, () => drillSweep.Completed);
        
        _stateMachine.AddAnyTransition(death, () => cores.All(x => x.Destroyed));
        
        var dieEvent = new ActionState().OnEnter(() => OnDie.Invoke());
        _stateMachine.AddTransition(death, dieEvent, () => death.Completed);
        
        _stateMachine.SetState(idle);
    }

    private void Update()
    {
        _rand = UnityEngine.Random.Range(0, 100);
        _stateMachine.Tick();
    }
}
