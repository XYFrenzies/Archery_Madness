using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class BowController : MonoBehaviour
{
    public Transform ArrowRest;
    public Transform KnockRest;
    public float SnapDistanceThreshold;

    public void Start()
    {
        InputManager.Instance.LeftController.TryGetBinding(
            XRButton.Grip,
            PressType.Continuous, 
            out m_LeftHandBinding_GripHold);

        InputManager.Instance.RightController.TryGetBinding(
            XRButton.Trigger, 
            PressType.Begin,
            out m_RightHandBindingTriggerBegin);
    }

    private void Update()
    {
        if ( m_ArrowKnocked )
        {
            if (false)
            {

            }
        }
        else
        {
            if (false)
            {

            }
        }
    }

    private void OnTriggerEnter(Collider a_Other)
    {
        if (!a_Other.gameObject.TryGetComponent(out Arrow arrow ))
        {
            return;
        }

        m_ArrowKnocked = true;
        m_KnockedArrow = arrow;
    }

    private Ray GenerateArrowRay()
    {
        return new Ray(KnockRest.position, (ArrowRest.position - KnockRest.position).normalized);
    }

    private float PerpindicularDistanceToRay(Ray a_Ray, Vector3 a_Point)
    {
        return Vector3.Cross(a_Ray.direction, a_Point - a_Ray.origin).magnitude;
    }

    private Vector3 ClosestPointOnRay(Ray a_Ray, Vector3 a_Position)
    {
        return Vector3.Project(a_Position - a_Ray.origin, a_Ray.direction) + a_Ray.origin;
    }

    private bool m_ArrowKnocked;
    private Arrow m_KnockedArrow;
    private XRBinding m_LeftHandBinding_GripHold;
    private XRBinding m_RightHandBindingTriggerBegin;
}