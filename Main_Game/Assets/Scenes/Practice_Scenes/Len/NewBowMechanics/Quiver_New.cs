using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Quiver_New : XRSocketInteractor
{
    public GameObject ArrowPrefab = null;
    private Vector3 AttachOffset = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();

        CreateAndSelectArrow();
        SetAttachOffset();
    }

    protected override void OnSelectExiting( XRBaseInteractable a_Interactable )
    {
        base.OnSelectExiting(a_Interactable);
        CreateAndSelectArrow();
    }

    protected override void OnSelectExited(XRBaseInteractable a_Interactable)
    {
        
    }

    private void CreateAndSelectArrow()
    {
        Arrow_New arrow = CreateArrow();
        SelectArrow( arrow );
    }

    private Arrow_New CreateArrow()
    {
        GameObject arrowObject = Instantiate( 
            ArrowPrefab, 
            transform.position - AttachOffset, 
            transform.rotation );

        return arrowObject.GetComponent< Arrow_New >();
    }

    private void SelectArrow( Arrow_New a_Arrow )
    {
        OnSelectEntered(a_Arrow);
        a_Arrow.OnSelectEntered(this);
    }

    private void SetAttachOffset()
    {
        if ( selectTarget is XRGrabInteractable interactable )
        {
            AttachOffset = interactable.attachTransform.localPosition;
        }
    }
}
