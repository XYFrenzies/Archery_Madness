using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetDock : MonoBehaviour
{
    public enum Direction
    {
        X_POS,
        Y_POS,
        Z_POS,
        X_NEG,
        Y_NEG,
        Z_NEG
    }

    public Transform DockPivot;
    public Transform TargetAnchor;
    public AudioSource AudioSource;

    public Target DockedTarget { get; private set; }
    public Direction RotationDirection
    {
        get
        {
            return m_RotationDirection;
        }
    }
    public bool IsStanding { get; private set; } = true;
    public bool IsTransitioning { get; private set; } = false;

    public TrackDock TrackDock
    {
        get
        {
            if ( m_TrackDock == null )
            {
                m_TrackDock = GetComponent< TrackDock >();
            }

            return m_TrackDock;
        }
        private set
        {
            m_TrackDock = value;
        }
    }

    private void Awake()
    {
        m_DegreesPerSecondDown = m_RotationAngle / m_TransitionTimeDown;
        m_DegreesPerSecondUp = m_RotationAngle / m_TransitionTimeUp;
        TrackDock = GetComponent< TrackDock >();
        IsStanding = m_IsInitiallyStanding;
        m_PersistantRotation = new Quaternion();
        AudioSource = GetComponent< AudioSource >();
    }

    public void TriggerFlipDown()
    {
        // remove !IsTransitioning check
        if ( !IsStanding )
        {
            return;
        }

        StartCoroutine( FlipDown() );
        IsTransitioning = true;
    }

    public void TriggerFlipDownAndDestroy()
    {
        StartCoroutine( FlipDownAndDestroy( true ) );
        IsTransitioning = true;
    }

    public void TriggerFlipUp()
    {
        if ( IsTransitioning || IsStanding )
        {
            return;
        }

        StartCoroutine( FlipUp() );
    }

    // Needs to change for object pools.
    //public void SpawnUITarget( Target_UI.UIButton a_ButtonType, bool a_WithFlip )
    //{
    //    if ( DockedTarget != null )
    //    {
    //        Destroy( DockedTarget.gameObject );
    //    }
    //
    //    GameObject targetToCreate = null;
    //
    //    switch ( a_ButtonType )
    //    {
    //        case Target_UI.UIButton.PLAY:
    //            {
    //                targetToCreate = GalleryController.Instance.TargetUIPrefab_Play;
    //                break;
    //            }
    //        case Target_UI.UIButton.EXIT:
    //            {
    //                targetToCreate = GalleryController.Instance.TargetUIPrefab_Exit;
    //                break;
    //            }
    //    }
    //
    //    if ( a_WithFlip )
    //    {
    //        IsStanding = false;
    //        SetRotation( RotationDirection, m_RotationAngle );
    //    }
    //
    //    GameObject newTarget = Instantiate( targetToCreate, DockPivot.transform );
    //    newTarget.transform.localPosition = TargetAnchor.localPosition;
    //    newTarget.transform.localRotation = TargetAnchor.localRotation;
    //    newTarget.transform.localScale = TargetAnchor.localScale;
    //    DockedTarget = GetTargetFromGameObject( newTarget );
    //    DockedTarget.TargetDock = this;
    //
    //    if ( a_WithFlip )
    //    {
    //        StartCoroutine( FlipUp() );
    //    }
    //}

    public void SpawnUITarget( Target_UI.UIButton a_ButtonType, bool a_WithFlip )
    {
        if ( DockedTarget != null )
        {
            Destroy( DockedTarget.gameObject );
        }

        GameObject pooledUIObject = null;
        Target_UI pooledUI = null;


        switch ( a_ButtonType )
        {
            case Target_UI.UIButton.PLAY:
                {
                    if ( GalleryController.Instance.PoolPlayUI.TrySpawn( out Target_UI uiPlay ) )
                    {
                        pooledUIObject = uiPlay.gameObject;
                        pooledUI = uiPlay;
                    }
                    else
                    {
                        return;
                    }

                    break;
                }
            case Target_UI.UIButton.EXIT:
                {
                    if ( GalleryController.Instance.PoolExitUI.TrySpawn( out Target_UI uiExit ) )
                    {
                        pooledUIObject = uiExit.gameObject;
                        pooledUI = uiExit;
                    }
                    else
                    {
                        return;
                    }

                    break;
                }
        }

        if ( a_WithFlip )
        {
            IsStanding = false;
            SetRotation( RotationDirection, m_RotationAngle );
        }

        pooledUIObject.transform.SetParent( DockPivot.transform );
        pooledUIObject.transform.localPosition = TargetAnchor.localPosition;
        pooledUIObject.transform.localRotation = TargetAnchor.localRotation;
        pooledUIObject.transform.localScale = TargetAnchor.localScale;
        DockedTarget = pooledUI;
        DockedTarget.TargetDock = this;

        if ( a_WithFlip )
        {
            StartCoroutine( FlipUp() );
        }
    }

    // Needs change for object pooling.
    //public void SpawnTarget( Target.TargetType a_TargetType, bool a_WithFlip, float a_FlipDownTimer = -1, bool a_NotifyOfKill = false )
    //{
    //    if ( DockedTarget != null )
    //    {
    //        DockedTarget.DestroyTarget();
    //    }
    //
    //    GameObject targetToCreate = null;
    //
    //    switch ( a_TargetType )
    //    {
    //        case Target.TargetType.WOOD:
    //            {
    //                targetToCreate = GalleryController.Instance.TargetPrefab_Wood;
    //                break;
    //            }
    //        case Target.TargetType.FIRE:
    //            {
    //                targetToCreate = GalleryController.Instance.TargetPrefabs_Fire;
    //                break;
    //            }
    //        case Target.TargetType.GLASS:
    //            {
    //                targetToCreate = GalleryController.Instance.TargetPrefab_Glass;
    //                break;
    //            }
    //    }
    //
    //    if ( a_WithFlip )
    //    {
    //        IsStanding = false;
    //        SetRotation( RotationDirection, m_RotationAngle );
    //    }
    //
    //    GameObject newTarget = Instantiate( targetToCreate, DockPivot.transform );
    //    newTarget.transform.localPosition = TargetAnchor.localPosition;
    //    newTarget.transform.localRotation = TargetAnchor.localRotation;
    //    DockedTarget = GetTargetFromGameObject( newTarget );
    //    DockedTarget.TargetDock = this;
    //
    //    if ( a_WithFlip )
    //    {
    //        StartCoroutine( FlipUp( a_FlipDownTimer, a_NotifyOfKill ) );
    //    }
    //}

    public void SpawnTarget( Target.TargetType a_TargetType, bool a_WithFlip, float a_FlipDownTimer = -1, bool a_NotifyOfKill = false )
    {
        if ( DockedTarget != null )
        {
            DockedTarget.DestroyTarget();
        }

        GameObject pooledObject = null;
        Target pooledTarget = null;

        switch ( a_TargetType )
        {
            case Target.TargetType.WOOD:
                {
                    if ( GalleryController.Instance.PoolWoodBird.TrySpawn( out Target_WoodBird woodBird ) )
                    {
                        pooledObject = woodBird.gameObject;
                        pooledTarget = woodBird;
                    }
                    else
                    {
                        return;
                    }

                    break;
                }
            case Target.TargetType.FIRE:
                {
                    if ( GalleryController.Instance.PoolFireBird.TrySpawn( out Target_FireBird fireBird ) )
                    {
                        pooledObject = fireBird.gameObject;
                        pooledTarget = fireBird;
                    }
                    else
                    {
                        return;
                    }

                    break;
                }
            case Target.TargetType.GLASS:
                {
                    if ( GalleryController.Instance.PoolGlassBird.TrySpawn( out Target_GlassBird glassBird ) )
                    {
                        pooledObject = glassBird.gameObject;
                        pooledTarget = glassBird;
                    }
                    else
                    {
                        return;
                    }

                    break;
                }
        }

        if ( a_WithFlip )
        {
            IsStanding = false;
            SetRotation( RotationDirection, m_RotationAngle );
        }

        //GameObject newTarget = Instantiate( pooledTarget, DockPivot.transform );
        pooledObject.transform.SetParent( DockPivot.transform );
        pooledObject.transform.localPosition = TargetAnchor.localPosition;
        pooledObject.transform.localRotation = TargetAnchor.localRotation;
        pooledObject.transform.localScale = TargetAnchor.localScale;
        DockedTarget = pooledTarget;
        DockedTarget.TargetDock = this;

        if ( a_WithFlip )
        {
            StartCoroutine( FlipUp( a_FlipDownTimer, a_NotifyOfKill ) );
        }
    }

    public void DestroyTarget()
    {
        if ( DockedTarget != null )
        {
            switch ( DockedTarget.Type )
            {
                case Target.TargetType.UI:
                    {
                        Target_UI ui = DockedTarget as Target_UI;

                        if ( ui.ButtonType == Target_UI.UIButton.PLAY )
                        {
                            DockedTarget.GetComponent< ShatterObject >().SetOnDisable( DisableUIPlay );
                        }
                        else if ( ui.ButtonType == Target_UI.UIButton.EXIT )
                        {
                            DockedTarget.GetComponent< ShatterObject >().SetOnDisable( DisableUIExit );
                        }

                        break;
                    }
                case Target.TargetType.WOOD:
                    {
                        DockedTarget.GetComponent< ShatterObject >().SetOnDisable( DisableWoodbird );

                        break;
                    }
                case Target.TargetType.FIRE:
                    {
                        DockedTarget.GetComponent< ShatterObject >().SetOnDisable( DisableFirebird );

                        break;
                    }
                case Target.TargetType.GLASS:
                    {
                        DockedTarget.GetComponent< ShatterObject >().SetOnDisable( DisableFirebird );

                        break;
                    }
                default:
                    {
                        return;
                    }
            }

            DockedTarget.DestroyTarget();
        }
    }

    private void DisableUIPlay( GameObject a_GameObject )
    {
        Target_UI target = a_GameObject.GetComponent< Target_UI >();
        GalleryController.Instance.PoolPlayUI.Despawn( target );
    }

    private void DisableUIExit( GameObject a_GameObject )
    {
        Target_UI target = a_GameObject.GetComponent< Target_UI >();
        GalleryController.Instance.PoolPlayUI.Despawn( target );
    }

    private void DisableFirebird( GameObject a_GameObject )
    {
        Target_FireBird target = a_GameObject.GetComponent< Target_FireBird >();
        GalleryController.Instance.PoolFireBird.Despawn( target );
    }

    private void DisableGlassbird( GameObject a_GameObject )
    {
        Target_GlassBird target = a_GameObject.GetComponent< Target_GlassBird >();
        GalleryController.Instance.PoolGlassBird.Despawn( target );
    }

    private void DisableWoodbird( GameObject a_GameObject )
    {
        Target_WoodBird target = a_GameObject.GetComponent< Target_WoodBird >();
        GalleryController.Instance.PoolWoodBird.Despawn( target );
    }

    public IEnumerator FlipDown()
    {
        AudioSource.clip = SoundPlayer.Instance.GetClip( "Actuator" );
        AudioSource.Play();
        IsTransitioning = true;
        IsStanding = false;

        //----------------------------------------------
        Direction rotationDirection = RotationDirection;
        float accumulativeAngle = 0.0f;

        while ( accumulativeAngle < m_RotationAngle )
        {
            yield return new WaitForEndOfFrame();
            accumulativeAngle += m_DegreesPerSecondDown * Time.deltaTime;
            SetRotation( rotationDirection, accumulativeAngle );
        }

        SetRotation( rotationDirection, m_RotationAngle );
        //----------------------------------------------

        IsTransitioning = false;
    }

    // Needs to pool
    //public IEnumerator FlipDownAndDestroy( bool a_NotifyOfKill )
    //{
    //    AudioSource.clip = SoundPlayer.Instance.GetClip( "Actuator" );
    //    AudioSource.Play();
    //
    //    IsTransitioning = true;
    //    IsStanding = false;
    //
    //    //----------------------------------------------
    //    Direction rotationDirection = RotationDirection;
    //    float accumulativeAngle = 0.0f;
    //
    //    while ( accumulativeAngle < m_RotationAngle )
    //    {
    //        yield return new WaitForEndOfFrame();
    //        accumulativeAngle += m_DegreesPerSecondDown * Time.deltaTime;
    //        SetRotation( rotationDirection, accumulativeAngle );
    //    }
    //
    //    SetRotation( rotationDirection, m_RotationAngle );
    //    //----------------------------------------------
    //
    //    IsTransitioning = false;
    //
    //    if ( a_NotifyOfKill )
    //    {
    //        GalleryController.Instance.NotifyOfKill();
    //    }
    //
    //    Destroy( DockedTarget.gameObject );
    //}

    public IEnumerator FlipDownAndDestroy( bool a_NotifyOfKill )
    {
        AudioSource.clip = SoundPlayer.Instance.GetClip( "Actuator" );
        AudioSource.Play();

        IsTransitioning = true;
        IsStanding = false;

        //----------------------------------------------
        Direction rotationDirection = RotationDirection;
        float accumulativeAngle = 0.0f;

        while ( accumulativeAngle < m_RotationAngle )
        {
            yield return new WaitForEndOfFrame();
            accumulativeAngle += m_DegreesPerSecondDown * Time.deltaTime;
            SetRotation( rotationDirection, accumulativeAngle );
        }

        SetRotation( rotationDirection, m_RotationAngle );
        //----------------------------------------------

        IsTransitioning = false;

        if ( a_NotifyOfKill )
        {
            GalleryController.Instance.NotifyOfKill();
        }

        //Destroy( DockedTarget.gameObject );
        
        // Added this
        if ( DockedTarget is Target_FireBird fireBird )
        {
            GalleryController.Instance.PoolFireBird.Despawn( fireBird );
        }
        else if ( DockedTarget is Target_GlassBird glassBird )
        {
            GalleryController.Instance.PoolGlassBird.Despawn( glassBird );
        }
        else if ( DockedTarget is Target_WoodBird woodBird )
        {
            GalleryController.Instance.PoolWoodBird.Despawn( woodBird );
        }
    }

    public IEnumerator FlipUp( float a_FlipDownTimer = -1, bool a_NotifyOfKill = false )
    {
        AudioSource.clip = SoundPlayer.Instance.GetClip( "Actuator" );
        AudioSource.Play();

        IsTransitioning = true;

        //----------------------------------------------
        Direction rotationDirection = RotationDirection;
        float accumulativeAngle = m_RotationAngle;

        while ( accumulativeAngle > 0 )
        {
            yield return new WaitForEndOfFrame();
            accumulativeAngle -= m_DegreesPerSecondUp * Time.deltaTime;

            SetRotation( rotationDirection, accumulativeAngle );
        }
        
        SetRotation( rotationDirection, 0.0f );
        //----------------------------------------------

        IsStanding = true;
        IsTransitioning = false;

        if ( a_FlipDownTimer != -1 )
        {
            yield return new WaitForSeconds( a_FlipDownTimer );
            StartCoroutine( FlipDownAndDestroy( a_NotifyOfKill ) );
        }
    }

    public Target GetTargetFromGameObject( GameObject a_GameObject )
    {
        Target target = a_GameObject.GetComponent< Target >();

        if ( target != null )
        {
            return target;
        }

        target = a_GameObject.GetComponentInChildren< Target >();
        return target;
    }

    private void SetRotation( Direction a_Direction, float a_Angle )
    {
        switch ( a_Direction )
        {
            case Direction.X_POS:
                {
                    m_PersistantRotation.eulerAngles = new Vector3( a_Angle, 0, 0 );
                    break;
                }
            case Direction.Y_POS:
                {
                    m_PersistantRotation.eulerAngles = new Vector3( 0, a_Angle, 0 );
                    break;
                }
            case Direction.Z_POS:
                {
                    m_PersistantRotation.eulerAngles = new Vector3( 0, 0, a_Angle );
                    break;
                }
            case Direction.X_NEG:
                {
                    m_PersistantRotation.eulerAngles = new Vector3( -a_Angle, 0, 0 );
                    break;
                }
            case Direction.Y_NEG:
                {
                    m_PersistantRotation.eulerAngles = new Vector3( 0, -a_Angle, 0 );
                    break;
                }
            case Direction.Z_NEG:
                {
                    m_PersistantRotation.eulerAngles = new Vector3( 0, 0, -a_Angle );
                    break;
                }
        }

        DockPivot.transform.localRotation = m_PersistantRotation;
    }

    #pragma warning disable 0649

    [ SerializeField ] private bool m_IsInitiallyStanding = true;
    [ SerializeField ] [ Range( 0.0f, m_MaxRotationAngle ) ] private float m_RotationAngle = 90.0f;
    [ SerializeField ] [ Range( 0.1f, m_MaxTransitionTime ) ] private float m_TransitionTimeDown = 2.0f;
    [ SerializeField ] [ Range( 0.1f, m_MaxTransitionTime ) ] private float m_TransitionTimeUp = 2.0f; 
    [ SerializeField ] private Direction m_RotationDirection;
    private TrackDock m_TrackDock;
    private float m_DegreesPerSecondDown;
    private float m_DegreesPerSecondUp;
    private Quaternion m_PersistantRotation;
    private const float m_MaxRotationAngle = 360.0f;
    private const float m_MaxTransitionTime = 10.0f;

    #pragma warning restore
}
