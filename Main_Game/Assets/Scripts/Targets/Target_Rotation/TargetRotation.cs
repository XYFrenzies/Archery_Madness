using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRotation : MonoBehaviour
{
    [Tooltip("If the time is 0, then the target will stay down until the animation plays.")]
    [SerializeField] private bool toStandImmediately = false;
    [SerializeField] private bool startDown = false;
    [SerializeField] private float timerToStand = 5.0f;
    private Animation animationFall;
    private bool hasBeenHit;
    private bool isStanding;
    
    private float deltaTime = 0;
    private void Start()
    {
        animationFall = GetComponentInParent<Animation>();
        //if (startDown)
        //{

        //}
        hasBeenHit = false;
        isStanding = true;
    }
    private void Update()
    {
        //Quick escape from class if collision has not been made.
        if (!hasBeenHit)
            return;
        //If the designer is using a timer.
        if (timerToStand > 0)
        {
            deltaTime += Time.deltaTime;
            //Determines the amount of time that is input from the designer by the amount of deltatime.
            if (timerToStand <= deltaTime)
            {
                animationFall.Play("Target_Stand_Back_Up");
                deltaTime = 0;
                isStanding = true;
                hasBeenHit = false;
            }
        }
    }

    //If the target is collided by another object.
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
