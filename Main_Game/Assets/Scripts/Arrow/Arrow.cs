using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class Arrow : XRGrabInteractable
{
    public enum ArrowType
    {
        BROAD,
        HAMMER,
        WATER
    }

    [ Header( "Settings" ) ]
    public float Speed = 2000.0f;

    [ Header( "Hit" ) ]
    public Transform Tip = null;
    public LayerMask LayerMask = Physics.IgnoreRaycastLayer;

    
    public ArrowType Type { get; protected set; }
    public Target.TargetType IntendedTarget { get; protected set; }
    public Vector3 InitialPosition { get { return m_InitialPosition; } }

    protected override void Awake()
    {
        base.Awake();
        m_Collider = GetComponent< Collider >();
        m_Rigidbody = GetComponent< Rigidbody >();
    }

    protected override void OnSelectEntering( SelectEnterEventArgs a_Args )
    {
        if ( a_Args.interactor is XRDirectInteractor )
        {
            Clear();
        }

        base.OnSelectEntering( a_Args );
    }

    private void Clear()
    {
        SetLaunch( false );
        TogglePhysics( true );
    }

    protected override void OnSelectExited( SelectExitEventArgs a_Args )
    {
        base.OnSelectExited( a_Args );

        if ( a_Args.interactor is Notch notch )
        {
            Launch( notch );
        }
    }

    private void Launch( Notch a_Notch )
    {
        if ( a_Notch.IsReady )
        {
            SetLaunch( true );
            UpdateLastPosition();
            SetInitialPosition( Tip.position );
            ApplyForce( a_Notch.PullMeasurer );
        }
    }

    private void SetLaunch( bool a_Value )
    {
        m_Collider.isTrigger = a_Value;
        m_Launched = a_Value;
    }

    private void UpdateLastPosition()
    {
        m_LastPosition = Tip.position;
    }

    private void ApplyForce( PullMeasurer a_PullMeasurer )
    {
        float power = a_PullMeasurer.PullAmount;
        Vector3 force = transform.forward * ( power * Speed );
        m_Rigidbody.AddForce( force );
    }

    public override void ProcessInteractable( XRInteractionUpdateOrder.UpdatePhase a_UpdatePhase )
    {
        base.ProcessInteractable( a_UpdatePhase );

        if ( m_Launched )
        {
            if ( a_UpdatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic )
            {
                if ( CheckForCollision() )
                {
                    m_Launched = false;
                }

                UpdateLastPosition();
            }

            if ( a_UpdatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed )
            {
                SetDirection();
            }
        }
    }

    private void SetDirection()
    {
        if ( m_Rigidbody.velocity.z > 0.5f )
        {
            transform.forward = m_Rigidbody.velocity;
        }
    }

    private void SetInitialPosition( Vector3 a_InitalPosition )
    {
        m_InitialPosition = a_InitalPosition;
    }

    private bool CheckForCollision()
    {
        if ( Physics.Linecast( m_LastPosition, Tip.position, out RaycastHit hit, LayerMask ) )
        {
            TogglePhysics( false );
            ChildArrow( hit );
            CheckForHittable( hit );
        }

        return hit.collider != null;
    }

    private void TogglePhysics( bool a_Value )
    {
        m_Rigidbody.isKinematic = !a_Value;
        m_Rigidbody.useGravity = a_Value;
    }

    private void ChildArrow( RaycastHit a_Hit )
    {
        Transform newParent = a_Hit.collider.transform;
        transform.SetParent( newParent );
    }

    private void CheckForHittable( RaycastHit a_Hit )
    {
        GameObject hitObject = a_Hit.transform.gameObject;
        IArrowHittable hittable = hitObject ? hitObject.GetComponent< IArrowHittable >() : null;

        if ( hittable != null )
        {
            hittable.OnArrowHit( this );
        }

        if ( hittable is Target )
        {
            GameStateManager.Instance.RegisterShot( new ContactScenario( this, hittable as Target ) );
        }
        else
        {
            GameStateManager.Instance.RegisterShot( new ContactScenario( this, Tip.position ) );
        }
    }
    
    private Collider m_Collider;
    private Rigidbody m_Rigidbody;

    private Vector3 m_InitialPosition = Vector3.zero;
    private Vector3 m_LastPosition = Vector3.zero;
    private bool m_Launched = false;
}
