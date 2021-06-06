using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PullMeasurer : XRBaseInteractable
{
    public class PullEvent : UnityEvent< Vector3, float > { }
    public PullEvent Pulled = new PullEvent();
    public Transform Start = null;
    public Transform End = null;

    private float m_PullAmount = 0.0f;
    public float PullAmount => m_PullAmount;
    private XRBaseInteractor m_PullingInteractor = null;

    protected override void OnSelectEntered( SelectEnterEventArgs a_Args )
    {
        base.OnSelectEntered( a_Args );
        m_PullingInteractor = a_Args.interactor;
    }

    protected override void OnSelectExited( SelectExitEventArgs a_Args )
    {
        base.OnSelectExited( a_Args );
        m_PullingInteractor = null;

        SetPullValues( Start.position, 0.0f );
    }

    public override void ProcessInteractable( XRInteractionUpdateOrder.UpdatePhase a_UpdatePhase )
    {
        base.ProcessInteractable( a_UpdatePhase );

        if ( isSelected )
        {
            if ( a_UpdatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic )
            {
                CheckForPull();
            }
        }
    }

    private void CheckForPull()
    {
        Vector3 interactorPosition = m_PullingInteractor.transform.position;
        float newPullAmount = CalculatePull( interactorPosition );
        Vector3 newPullPosition = CalculatePosition( newPullAmount );
        SetPullValues( newPullPosition, newPullAmount );
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

    private Vector3 CalculatePosition( float a_Amount )
    {
        return Vector3.Lerp( Start.position, End.position, a_Amount );
    }

    private void SetPullValues( Vector3 a_NewPullPosition, float a_NewPullAmount )
    {
        if ( a_NewPullAmount != m_PullAmount )
        {
            m_PullAmount = a_NewPullAmount;
            Pulled?.Invoke( a_NewPullPosition, a_NewPullAmount );
        }
    }

    public override bool IsSelectableBy( XRBaseInteractor a_Interactor )
    {
        return base.IsSelectableBy( a_Interactor ) && 
            IsDirectInteractor( a_Interactor );
    }

    private bool IsDirectInteractor( XRBaseInteractor a_Interactor )
    {
        return a_Interactor is XRDirectInteractor;
    }

    private void OnDrawGizmos()
    {
        if ( Start && End )
        {
            Gizmos.DrawLine( Start.position, End.position );
        }
    }
}
