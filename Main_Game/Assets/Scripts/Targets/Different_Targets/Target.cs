using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour, IResettable
{
    public enum TargetType
    {
        WOOD,
        FIRE,
        GLASS
    }

    public TargetType Type
    {
        get
        {
            return m_TargetType;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( !collision.gameObject.TryGetComponent( out Arrow arrow ) )
        {
            return;
        }

        OnArrowContact(arrow);
    }

    public void OnReset()
    {
        // This is called when object pool is asked to reset this object.
    }

    public virtual void OnArrowContact(Arrow a_ContactingArrow)
    {
        // Called if an arrow contacts this target.
    }

    public void AttachToSlideRail(ConfigurableJoint a_SlideRailJoint)
    {
        m_SlideRailJoint = a_SlideRailJoint;
    }

    public virtual void OnActivate()
    {
        // Called when target is spawned from the object pool.
    }

    public virtual void OnDeactivate()
    {
        // Called when target is despawned from the object pool.
    }

    protected TargetType m_TargetType;
    private ConfigurableJoint m_SlideRailJoint;
}
