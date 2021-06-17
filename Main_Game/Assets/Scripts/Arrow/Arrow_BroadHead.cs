using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Broadhead : Arrow
{
    private new void Awake()
    {
        Type = ArrowType.BROAD;
        IntendedTarget = Target.TargetType.GLASS;
        
        base.Awake();
    }
}
