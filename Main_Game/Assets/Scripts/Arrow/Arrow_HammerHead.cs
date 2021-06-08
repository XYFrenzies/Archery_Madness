using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Hammerhead : Arrow
{
    private new void Awake()
    {
        base.Awake();
        Type = ArrowType.HAMMER;
        IntendedTarget = Target.TargetType.WOOD;
    }
}
