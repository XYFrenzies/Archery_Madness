using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Broadhead : Arrow
{
    public ObjectPool< Arrow > Pool;

    private new void Awake()
    {
        Type = ArrowType.BROAD;
        IntendedTarget = Target.TargetType.GLASS;
        GetComponent< ShatterObject >()?.SetOnDisable( ResetExplosion );
        
        base.Awake();
    }

    public override void ResetExplosion( GameObject gameObject )
    {
        Destroy( gameObject );
    }
}
