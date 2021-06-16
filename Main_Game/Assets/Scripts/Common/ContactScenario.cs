using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ContactScenario
{
    public Arrow Arrow { get; private set; }
    public Target Target { get; private set; }
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
        if ( HitCorrectTarget )
        {
            return ( int )( TravelDistance * 4 );
        }
        else if ( HitTarget )
        {
            return -1;
        }
        else
        {
            return -20;
        }
    }
}
