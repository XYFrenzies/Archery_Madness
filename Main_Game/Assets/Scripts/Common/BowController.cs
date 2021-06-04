using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class BowController : MonoBehaviour
{
    public Transform ShootPoint;
    public GameObject Ball;

    public void Start()
    {
        InputManager.Instance.LeftController.TryGetBinding(
            XRButton.Grip,
            PressType.Continuous, 
            out m_LeftHandBinding_GripHold);

        InputManager.Instance.RightController.TryGetBinding(
            XRButton.Trigger, 
            PressType.Begin,
            out m_RightHandBindingTriggerHold);
    }

    private void Update()
    {
        if ( m_RightHandBindingTriggerHold.Active )
        {
            Instantiate( Ball ).transform.position = ShootPoint.transform.position;
        }
    }

    private XRBinding m_LeftHandBinding_GripHold;
    private XRBinding m_RightHandBindingTriggerHold;
}