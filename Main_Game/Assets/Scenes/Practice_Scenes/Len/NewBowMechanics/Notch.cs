using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Notch : XRSocketInteractor
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnSelectEntered( XRBaseInteractable a_Interactable )
    {
        base.OnSelectEntered( a_Interactable );
    }

    private void StoreArrow( XRBaseInteractable a_Interactable )
    {

    }

    private void TryToReleaseArrow( XRBaseInteractor a_Interactor )
    {

    }

    private void ForceDeselect()
    {

    }

    private void ReleaseArrow()
    {

    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }
}
