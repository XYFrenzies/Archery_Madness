using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[ RequireComponent( typeof( PullMeasurer ) ) ]
public class Notch : XRSocketInteractor
{
    [ Range( 0.0f, 1.0f ) ] public float ReleaseThreshold = 0.25f;

    public PullMeasurer PullMeasurer { get; private set; } = null;
    public bool IsReady { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();
        PullMeasurer = GetComponent< PullMeasurer >();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        PullMeasurer.selectExited.AddListener( ReleaseArrow );
        PullMeasurer.Pulled.AddListener( MoveAttach );
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        PullMeasurer.selectExited.RemoveListener( ReleaseArrow );
        PullMeasurer.Pulled.RemoveListener( MoveAttach );
    }

    public void ReleaseArrow( SelectExitEventArgs a_Args )
    {
        if ( selectTarget )
        {
            interactionManager.SelectExit( this, selectTarget );
        }
    }

    public void MoveAttach( Vector3 a_PullPosition, float a_PullAmount )
    {
        attachTransform.position = a_PullPosition;
    }

    public void SetReady( BaseInteractionEventArgs a_Args )
    {
        IsReady = a_Args.interactable.isSelected;
    }

    public override bool CanSelect( XRBaseInteractable a_Interactable )
    {
        return base.CanSelect( a_Interactable ) && 
            CanHover( a_Interactable ) && 
            IsArrow( a_Interactable ) && IsReady;
    }

    private bool IsArrow( XRBaseInteractable a_Interactable )
    {
        return a_Interactable is Arrow;
    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }

    public override bool requireSelectExclusive => false;
}
