using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandController : XRDirectInteractor
{
    public enum Side
    {
        LEFT,
        RIGHT
    }

    public Side HandSide;
}
