using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_FireBird : Target
{
    private void Awake()
    {
        Type = TargetType.FIRE;
    }

    public override void OnArrowHit( Arrow a_Arrow )
    {
        base.OnArrowHit(a_Arrow);
    }
}
