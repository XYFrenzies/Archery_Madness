using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bow_New : XRGrabInteractable
{
    private Animator m_Animator = null;
    private Puller m_Puller = null;
    protected override void Awake()
    {
        base.Awake();
        m_Animator = GetComponent< Animator >();
        m_Puller = GetComponentInChildren< Puller >();
    }

    public override void ProcessInteractable( XRInteractionUpdateOrder.UpdatePhase a_UpdatePhase )
    {
        base.ProcessInteractable( a_UpdatePhase );

        if ( a_UpdatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic )
        {
            if ( isSelected )
            {
                AnimateBow( m_Puller.PullAmount );
            }
        }
    }

    private void AnimateBow( float a_Value )
    {
        m_Animator.SetFloat( "Blend", a_Value );
    }
}
