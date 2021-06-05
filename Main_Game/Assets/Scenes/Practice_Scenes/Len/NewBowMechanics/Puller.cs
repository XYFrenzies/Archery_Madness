using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Puller : XRBaseInteractable
{
    public float PullAmount { get; private set; } = 0.0f;
    public Transform Start = null;
    public Transform End = null;
    private XRBaseInteractor PullingInteractor = null;
    protected override void OnSelectEntered( XRBaseInteractor a_Interactor )
    {
        base.OnSelectEntered( a_Interactor );
        PullingInteractor = a_Interactor;
    }

    protected override void OnSelectExited( XRBaseInteractor a_Interactor )
    {
        base.OnSelectExited( a_Interactor );
        PullingInteractor = a_Interactor;
        PullAmount = 0.0f;
    }

    public override void ProcessInteractable( XRInteractionUpdateOrder.UpdatePhase a_UpdatePhase )
    {
        base.ProcessInteractable( a_UpdatePhase );

        if ( a_UpdatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic )
        {
            if ( isSelected )
            {
                Vector3 pullPosition = PullingInteractor.transform.position;
                PullAmount = CalculatePull( pullPosition );
            }
        }
    }

    private float CalculatePull( Vector3 a_PullPosition )
    {
        Vector3 pullDirection = a_PullPosition - Start.position;
        Vector3 targetDirection = End.position - Start.position;
        float maxLength = targetDirection.magnitude;
        targetDirection.Normalize();
        float pullValue = Vector3.Dot( pullDirection, targetDirection ) / maxLength;

        return Mathf.Clamp( pullValue, 0.0f, 1.0f );
    }
}
