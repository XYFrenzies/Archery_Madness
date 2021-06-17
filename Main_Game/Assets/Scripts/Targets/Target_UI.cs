using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_UI : Target
{
    public enum UIButton
    {
        PLAY,
        EXIT
    }

    public UIButton ButtonType;

    private void Awake()
    {
        Type = TargetType.UI;
    }

    public override void DestroyTarget()
    {
        //gameObject.transform.DetachChildren();
        //gameObject.GetComponent< Rigidbody >().isKinematic = false;
        //GetComponent< DestructionController >()?.BlowUp();
        //if ( TryGetComponent( out ShatterObject shatter ) )
        //{
        //    shatter.enabled = true;
        //}

        //GetComponent< ShatterObject >()?.TriggerExplosion();
        base.DestroyTarget();
    }

    public override void OnArrowHit( Arrow a_Arrow )
    {
        base.OnArrowHit( a_Arrow );
    }
}
