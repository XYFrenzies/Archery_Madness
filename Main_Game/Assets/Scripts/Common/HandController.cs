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
    public XRDirectInteractor Interactor;

    private void Start()
    {
        switch (HandSide)
        {
            case Side.LEFT:
                {
                    InputManager.Instance.LeftController.TryGetBinding(
                        XRButton.Grip,
                        PressType.Begin,
                        out m_GripPressed);
                    break;
                }
            case Side.RIGHT:
                {
                    InputManager.Instance.RightController.TryGetBinding(
                        XRButton.Grip,
                        PressType.Begin,
                        out m_GripPressed);
                    break;
                }
        }
    }

    private void OnTriggerEnter(Collider a_Collider)
    {
        if (a_Collider.gameObject.TryGetComponent(out ArrowSpawner spawner))
        {
            spawner.OnPreInteract(this);
        }
    }

    private void OnTriggerExit(Collider a_Collider)
    {
        if (a_Collider.gameObject.TryGetComponent(out ArrowSpawner spawner))
        {
            spawner.OnPostInteract(this);
        }
    }

    private void OnTriggerStay(Collider a_Collider)
    {
        if (a_Collider.gameObject.TryGetComponent(out ArrowSpawner spawner) && m_GripPressed.Active)
        {
            spawner.OnInteract(this);
        }
    }

    private XRBinding m_GripPressed;
}
