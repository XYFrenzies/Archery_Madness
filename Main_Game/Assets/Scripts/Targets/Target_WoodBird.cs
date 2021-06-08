using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_WoodBird : Target
{
    private void Awake()
    {
        Type = TargetType.WOOD;
    }

    public override void OnArrowHit( Arrow a_Arrow )
    {
        base.OnArrowHit(a_Arrow);
    }
}
