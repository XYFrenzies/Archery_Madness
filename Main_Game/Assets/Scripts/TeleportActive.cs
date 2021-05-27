using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class TeleportActive : MonoBehaviour
{
    //The gameobject with the teleport script
    //[SerializeField] private GameObject m_teleportController = null;
    //This is the inputaction nreference that contains the button mapping data.
    [SerializeField] private InputActionReference m_teleportActiveReference = null;
    //[Space]
    //[Header("Teleport Events")]
    //This will group unity event calls that can be added to the inspector.
    [SerializeField] private UnityEvent onTeleportActivate = null;
    [SerializeField] private UnityEvent onTeleportCancel = null;
    // Start is called before the first frame update
    void Start()
    {
        //When an interaction occurs with the reference to active the teleportation, a callback will occur to activate.
        m_teleportActiveReference.action.performed += TeleportModeActivate;
        //When an interaction occurs with the reference to active the teleportation, a callback will occur to cancel.
        m_teleportActiveReference.action.canceled += TeleportModeCancel;
    }
    //This creates a series of events in the onTeleportactive/cancel action within the inspector
    private void TeleportModeActivate(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();
    private void TeleportModeCancel(InputAction.CallbackContext obj) => Invoke("DelayTeleportation", 0.1f);
    //Creates a delay for which the action is called after the cancalation of the call.
    private void DelayTeleportation() => onTeleportCancel.Invoke();
}
