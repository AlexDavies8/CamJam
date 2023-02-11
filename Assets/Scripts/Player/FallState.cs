using System.Collections;
using System.Collections.Generic;
using MultiState;
using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerControllerSettings settings, PlayerControllerState state) : base(settings, state) {}

    public override void OnEnter()
    {
        State.movementSmoothing = Settings.movementSmoothing * Settings.fallMovementSmoothingMultiplier;
        
        State.animator.Play(Settings.fallAnimation, 0);
    }

    public override void OnExit()
    {
        State.movementSmoothing = Settings.movementSmoothing;
        GameManager.Instance.GetGlobalComponent<ScreenShaker>().AddTrauma(Settings.impactScreenShake);
    }

}
