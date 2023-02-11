using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScriptUtils
{
    public static AnimationClip GetAnimationClip(this Animator animator, string name, int layer = 0)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }

        return null;
    }
}
