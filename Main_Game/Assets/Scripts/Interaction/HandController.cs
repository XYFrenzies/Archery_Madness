using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandController : MonoBehaviour
{
    public enum Side
    {
        LEFT,
        RIGHT
    }

    public Side HandSide;

    private void OnTriggerEnter(Collider a_Collider)
    {

    }

    private void OnTriggerExit(Collider a_Collider)
    {

    }

    private void OnTriggerStay(Collider a_Collider)
    {

    }
}
