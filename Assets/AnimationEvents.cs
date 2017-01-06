using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour {

    public delegate void AnimationEvent();
    public static event AnimationEvent OnSlashAnimationHit;

void SlashAnimationHitEvent()
    {
        print("event called");
        OnSlashAnimationHit();
    }
}
