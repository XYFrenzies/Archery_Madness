using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class Arrow : XRGrabInteractable, IResettable
{
    public enum ArrowType
    {
        NONE,
        BROAD,
        HAMMER,
        WATER
    }

    [ Header( "Settings" ) ]
    public float Speed = 2000.0f;

    public AudioSource AudioSource;

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
        m_ThisRigidbody = GetComponent< Rigidbody >();
        AudioSource = GetComponent< AudioSource >();
    }

    protected override void OnSelectEntering( SelectEnterEventArgs a_Args )
    {
        base.OnSelectEntering( a_Args );
        if ( a_Args.interactor is XRDirectInteractor )
        {
            Clear();
        }

        //base.OnSelectEntering( a_Args );
    }

    private void Clear()
    {
        SetLaunch( false );
        TogglePhysics( true );
    }

    public void DestroyArrow()
    {
        ShatterObject shatter = GetComponent< ShatterObject >();
        shatter.SetOnDisable( DisableArrow );
        transform.SetParent( null );
        shatter.TriggerExplosion();
    }

    private static void DisableArrow( GameObject a_GameObject )
    {
        Arrow arrow = a_GameObject.GetComponent< Arrow >();

        switch (arrow.Type)
        {
            case ArrowType.BROAD:
                {
                    Arrow_Broadhead broadhead = arrow as Arrow_Broadhead;
                    GameStateManager.Instance.QuiverBroadhead.ArrowPool.Despawn( broadhead );
                    break;
                }
            case ArrowType.HAMMER:
                {
                    Arrow_Hammerhead hammerhead = arrow as Arrow_Hammerhead;
                    GameStateManager.Instance.QuiverHammerhead.ArrowPool.Despawn( hammerhead );
                    break;
                }
            case ArrowType.WATER:
                {
                    Arrow_WaterBalloon waterhead = arrow as Arrow_WaterBalloon;
                    GameStateManager.Instance.QuiverWaterhead.ArrowPool.Despawn( waterhead );
                    break;
                }
            default:
                {
                    return;
                }
        }
    }

    public void TriggerDespawn( float a_Time )
    {
        Destroy( gameObject, a_Time );
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
            //AudioSource.clip = SoundPlayer.Instance.GetClip( "ArrowFlight" );
            //AudioSource.Play();
            //SoundPlayer.Instance.Play( "BowTwang", "Notch", 1.0f );
        }
    }

    private void SetLaunch( bool a_Value )
    {
        m_Collider.isTrigger = a_Value;
        m_Launched = a_Value;
    }

    private void UpdateLastPosition()
    {
        m_LastKnownPosition = Tip.position;
    }

    private void ApplyForce( PullMeasurer a_PullMeasurer )
    {
        float power = a_PullMeasurer.PullAmount;
        Vector3 force = transform.forward * ( power * Speed );
        m_ThisRigidbody.AddForce( force );
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
        if ( m_ThisRigidbody.velocity.z > 0.5f )
        {
            transform.forward = m_ThisRigidbody.velocity;
        }
    }

    private void SetInitialPosition( Vector3 a_InitalPosition )
    {
        m_InitialPosition = a_InitalPosition;
    }

    private bool CheckForCollision()
    {
        if ( Physics.Linecast( m_LastKnownPosition, Tip.position, out RaycastHit hit, LayerMask ) )
        {
            OnCollision( hit );
        }

        return hit.collider != null;
    }

    private void OnCollision( RaycastHit a_Hit )
    {
        TogglePhysics( false );
        ChildArrow( a_Hit );
        CheckForHittable( a_Hit );
        TriggerDespawn( 3.0f );
    }

    private void TogglePhysics( bool a_Value )
    {
        m_ThisRigidbody.isKinematic = !a_Value;
        m_ThisRigidbody.useGravity = a_Value;
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

    public void OnReset()
    {
        
    }

    private Collider m_Collider;
    private Rigidbody m_ThisRigidbody;

    private Vector3 m_InitialPosition = Vector3.zero;
    private Vector3 m_LastKnownPosition = Vector3.zero;
    private bool m_Launched = false;
}
