using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : MonoBehaviour, IResettable
{
    public enum ArrowType
    {
        BROAD,
        HAMMER,
        WATER
    }

    protected void Awake()
    {

    }

    private void OnCollisionEnter(Collision a_Collision)
    {
        // When arrow collides with an object.
    }

    public void OnReset()
    {
        // When arrow is reset.
    }

    public virtual void OnActivate()
    {

    }

    public virtual void OnDeactivate()
    {

    }

    public ArrowType Type
    {
        get
        {
            return m_ArrowType;
        }
    }

    protected ArrowType m_ArrowType;
    protected XRGrabInteractable m_GrabInteractable;
}
