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
    public GameObject SpawnableObject;


    public bool HoldingArrow
    {
        get
        {
            return m_HoldingArrow;
        }
    }

    public Arrow HeldArrow
    {
        get
        {
            return m_HeldArrow;
        }
    }

    private void Start()
    {
        switch (HandSide)
        {
            case Side.LEFT:
                {
                    if ( InputManager.Instance.LeftController == null )
                    {
                        return;
                    }

                    InputManager.Instance.LeftController.TryGetBinding(
                        XRButton.Grip,
                        PressType.Begin,
                        out m_GripPressed);
                    break;
                }
            case Side.RIGHT:
                {
                    if ( InputManager.Instance.RightController == null )
                    {
                        return;
                    }

                    InputManager.Instance.RightController.TryGetBinding(
                        XRButton.Grip,
                        PressType.Begin,
                        out m_GripPressed);
                    break;
                }
        }
    }

    private void Update()
    {
        //if ( m_GripPressed.Active )
        //{
        //    GameObject newObject = Instantiate(SpawnableObject);
        //    newObject.transform.position = transform.position;
        //    Interactor.interactionManager.ForceSelect(Interactor, newObject.GetComponent<XRGrabInteractable>());
        //}
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

    public void OnSetHeldArrow( Arrow a_Arrow )
    {
        if ( a_Arrow == null )
        {
            return;
        }

        m_HoldingArrow = true;
        m_HeldArrow = a_Arrow;
    }

    public void OnDropHeldArrow()
    {
        if ( m_HeldArrow == null )
        {
            return;
        }

        m_HeldArrow = null;
        m_HoldingArrow = false;
    }

    private bool m_HoldingArrow;
    private Arrow m_HeldArrow;
    private XRBinding m_GripPressed;
}
