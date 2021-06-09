using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRotation : Singleton<TargetRotation>
{
    [Tooltip("If the time is 0, then the target will stay down until the animation plays.")]
    [SerializeField] private bool toStandImmediately = false;
    [Tooltip("Works only at the beginning of the game.")]
    [SerializeField] private bool startDown = false;
    [SerializeField] private float timerToStand = 5.0f;
    private Animation animationFall;
    private bool hasBeenHit;
    private bool isStanding;

    private float deltaTime = 0;
    private void Start()
    {
        animationFall = GetComponentInParent<Animation>();
        if (startDown)
        {
            transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, 0);
        }
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
            //Determines the amount of time that is input from the designer by the amount of deltatime.
            deltaTime += Time.deltaTime;
            if (timerToStand <= deltaTime)
            {
                StandUpObject();
                deltaTime = 0;
                hasBeenHit = false;
            }
        }
    }
    public void SetRotation(Vector3 anglesOnAxis)
    {
        transform.rotation = new Quaternion(anglesOnAxis.x, anglesOnAxis.y, anglesOnAxis.z, 0);
    }
    public void StandUpObject()
    {
        animationFall.Play("Target_Stand_Back_Up");
        isStanding = true;
    }
    public void SetTimer(float a_timeToStand) 
    {
        timerToStand = a_timeToStand;
    }
    public void PlayAnimation(string animationName) 
    {
        animationFall.clip = animationFall.GetClip(animationName);
        animationFall.Play();
    }
    public void ObjectFall() 
    {
        animationFall.Play("Target_Fall");
        isStanding = false;
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
