using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Hammerhead : Arrow
{
    public ObjectPool< Arrow > Pool;

    private new void Awake()
    {
        Type = ArrowType.HAMMER;
        IntendedTarget = Target.TargetType.WOOD;
        GetComponent< ShatterObject >()?.SetOnDisable( ResetExplosion );
        
        base.Awake();
    }

    public override void ResetExplosion( GameObject gameObject )
    {
        Destroy( gameObject );
    }
}
