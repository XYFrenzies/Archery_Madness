using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_WaterBalloon : Arrow
{
    private new void Awake()
    {
        base.Awake();
        Type = ArrowType.WATER;
        IntendedTarget = Target.TargetType.FIRE;
    }
}
