using MultiState;
using UnityEngine;

public class AnimationState : IState
{
    private readonly Animator _animator;
    private readonly string _animationName;

    public AnimationState(Animator animator, string animationName)
    {
        _animator = animator;
        _animationName = animationName;
    }

    public bool Completed => _timer < 0f;

    private float _timer;
    
    public void OnEnter()
    {
        _animator.Play(_animationName, 0);
        _timer = _animator.GetAnimationClip(_animationName)?.length ?? 0f;
    }

    public void Tick()
    {
        _timer -= Time.deltaTime;
    }

    public void OnExit()
    {
    }
}