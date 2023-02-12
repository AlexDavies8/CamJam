using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimateOnPowerCore : MonoBehaviour
{
    [SerializeField] private List<PowerCore> _cores;
    [SerializeField] private Animator _animator;

    private void Update()
    {
        if (_cores.All(x => x.Destroyed)) _animator.enabled = true;
    }
}
