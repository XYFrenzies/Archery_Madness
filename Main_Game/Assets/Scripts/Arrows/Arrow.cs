using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Arrow : MonoBehaviour, IResettable
{
    public enum ArrowType
    {
        BROAD,
        HAMMER,
        WATER
    }

    private void OnCollisionEnter(Collision collision)
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
}
