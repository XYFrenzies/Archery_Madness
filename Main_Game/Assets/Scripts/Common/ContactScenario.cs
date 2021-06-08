using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ContactScenario
{
    public Arrow Arrow;
    public Target Target;
    public bool HitTarget { get; private set; }
    public bool HitCorrectTarget { get; private set; }
    public float TravelDistance { get; private set; }

    public ContactScenario( Arrow a_Arrow, Target a_Target )
    {
        Arrow = a_Arrow;
        Target = a_Target;
        HitTarget = true;
        HitCorrectTarget = a_Arrow.IntendedTarget == a_Target.Type;
        TravelDistance = Vector3.Distance( a_Arrow.InitialPosition, a_Target.transform.position );
    }

    public ContactScenario( Arrow a_Arrow, Vector3 a_ContactPosition )
    {
        Arrow = a_Arrow;
        Target = null;
        HitTarget = false;
        HitCorrectTarget = false;
        TravelDistance = Vector3.Distance( a_Arrow.InitialPosition, a_ContactPosition );
    }

    public int ResultantScore()
    {
        // Define scoring parameters.
        return 0;
    }
}
