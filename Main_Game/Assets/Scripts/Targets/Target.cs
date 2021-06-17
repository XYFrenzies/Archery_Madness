using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour, IResettable, IArrowHittable
{
    public enum TargetType
    {
        NONE,
        UI,
        WOOD,
        FIRE,
        GLASS
    }

    public Transform ParentMostTransform;
    public TargetDock TargetDock;
    public Collider Collider;

    public TargetType Type { get; protected set; }

    public virtual void DestroyTarget()
    {
        //transform.SetParent( null );
        //GetComponent< DestructionController >()?.BlowUp();
        transform.SetParent( null );
        GetComponent< ShatterObject >()?.TriggerExplosion();
    }

    public void EnableCollider()
    {
        if ( Collider != null )
        {
            Collider.enabled = true;
        }
    }

    public void DisableCollider()
    {
        if ( Collider != null )
        {
            Collider.enabled = false;
        }
    }

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
}
