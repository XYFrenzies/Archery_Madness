using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour, IResettable, IArrowHittable
{
    public enum TargetType
    {
        WOOD,
        FIRE,
        GLASS
    }

    public TargetType Type { get; protected set; }

    public void OnReset()
    {
        // This is called when object pool is asked to reset this object.
    }

    public virtual void OnActivate()
    {
        // Called when target is spawned from the object pool.
    }

    public virtual void OnDeactivate()
    {
        // Called when target is despawned from the object pool.
    }

    public virtual void OnArrowHit( Arrow a_Arrow )
    {
        // Do something when arrow hits this object.
    }

    private ConfigurableJoint m_SlideRailJoint;
}
