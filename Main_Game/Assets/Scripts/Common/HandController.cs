using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour, IInteractable
{
    public enum Side
    {
        LEFT,
        RIGHT
    }

    public void Interact(IInteractable a_Other)
    {

    }

    public void UnInteract(IInteractable a_Other)
    {

    }

    public Side HandSide
    {
        get
        {
            return m_HandSide;
        }
    }

    private Side m_HandSide;
}
