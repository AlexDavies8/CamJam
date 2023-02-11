using System.Collections;
using System.Collections.Generic;
using MultiState;
using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerControllerSettings settings, PlayerControllerState state) : base(settings, state) {}

    public override void OnEnter()
    {
        Settings.movementSmoothing *= Settings.fallMovementSmoothingMultiplier;
    }

    public override void OnExit()
    {
        Settings.movementSmoothing /= Settings.fallMovementSmoothingMultiplier;
        GameManager.Instance.GetGlobalComponent<ScreenShaker>().AddTrauma(Settings.impactScreenShake);
    }

}
