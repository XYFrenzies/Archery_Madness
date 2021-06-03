using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRotation : MonoBehaviour
{
    [Tooltip("If the time is 0, then the target will stay down until the animation plays.")]
    [SerializeField] private float timerToStand = 5.0f;
    private Animation animationFall;
    private bool hasBeenHit;
    private bool isStanding;
    private float deltaTime = 0;
    private void Start()
    {
        animationFall = GetComponentInParent<Animation>();
        hasBeenHit = false;
        isStanding = true;
    }
    private void Update()
    {
        if (!hasBeenHit)
            return;
        if (timerToStand > 0)
        {
            deltaTime += Time.deltaTime;
            if (timerToStand <= deltaTime)
            {
                animationFall.Play("Target_Stand_Back_Up");
                deltaTime = 0;
                isStanding = true;
                hasBeenHit = false;
            }
        }
    }
    private void OnCollisionEnter(Collision _)
    {
        if (isStanding)
        {
            animationFall.clip = animationFall.GetClip("Target_Fall");
            animationFall.Play();
            isStanding = false;
            hasBeenHit = true;
        }

    }
}
