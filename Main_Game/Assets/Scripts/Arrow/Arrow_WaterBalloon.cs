using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_WaterBalloon : Arrow
{
    public ObjectPool< Arrow > Pool;

    private new void Awake()
    {
        Type = ArrowType.WATER;
        IntendedTarget = Target.TargetType.FIRE;
        GetComponent< ShatterObject >()?.SetOnDisable( ResetExplosion );
        
        base.Awake();
    }

    public override void ResetExplosion( GameObject gameObject )
    {
        Destroy( gameObject );
    }
}
