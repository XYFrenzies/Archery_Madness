using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_GlassBird : Target
{
    private void Awake()
    {
        Type = TargetType.GLASS;
    }

    public override void OnArrowHit( Arrow a_Arrow )
    {
        base.OnArrowHit(a_Arrow);
    }
}
