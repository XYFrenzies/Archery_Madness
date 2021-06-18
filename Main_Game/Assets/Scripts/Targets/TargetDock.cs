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

    public Target DockedTarget { get; set; }
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
            DockedTarget.DestroyTarget();
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

    public IEnumerator New_ShowTarget( Target.TargetType a_TargetType, float a_FlipDownTimer )
    {
        Target pooledTarget = null;

        // Attempt to get from pool.
        switch ( a_TargetType )
        {
            case Target.TargetType.WOOD:
                {
                    pooledTarget = Instantiate( GalleryController.Instance.PersistantWoodBird ).GetComponent< Target >();
                    break;
                }
            case Target.TargetType.FIRE:
                {
                    pooledTarget = Instantiate( GalleryController.Instance.PersistantFireBird ).GetComponent< Target >();
                    break;
                }
            case Target.TargetType.GLASS:
                {
                    pooledTarget = Instantiate( GalleryController.Instance.PersistantGlassBird ).GetComponent< Target >();
                    break;
                }
        }
        {
            // If a pooled object could be pooled.
            IsStanding = false;
            SetRotation( RotationDirection, m_RotationAngle );
            pooledTarget.transform.SetParent( DockPivot.transform );
            pooledTarget.transform.localPosition = TargetAnchor.localPosition;
            pooledTarget.transform.localRotation = TargetAnchor.localRotation;
            pooledTarget.transform.localScale = TargetAnchor.localScale;
            DockedTarget = pooledTarget;
            pooledTarget.gameObject.SetActive( true );
            DockedTarget.TargetDock = this;
            GalleryController.Instance.NotifyOfRespawn();

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

            // Wait for 5 seconds standing.
            float elapsedTime = 0.0f;

            while ( DockedTarget != null && elapsedTime < a_FlipDownTimer )
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            AudioSource.clip = SoundPlayer.Instance.GetClip( "Actuator" );
            AudioSource.Play();

            IsTransitioning = true;
            IsStanding = false;

            //----------------------------------------------
            rotationDirection = RotationDirection;
            accumulativeAngle = 0.0f;

            while ( accumulativeAngle < m_RotationAngle )
            {
                yield return new WaitForEndOfFrame();
                accumulativeAngle += m_DegreesPerSecondDown * Time.deltaTime;
                SetRotation( rotationDirection, accumulativeAngle );
            }

            SetRotation( rotationDirection, m_RotationAngle );

            //----------------------------------------------

            IsTransitioning = false;

            if ( DockedTarget != null )
            {
                Destroy( DockedTarget.gameObject );
                DockedTarget = null;
                GalleryController.Instance.NotifyOfKill();
            }            
        }
    }

    public IEnumerator New_ShowUI( Target_UI.UIButton a_ButtonType )
    {
        Direction rotationDirection;
        float accumulativeAngle;

        // If a button is already docked, rotate, and despawn it.
        if ( DockedTarget != null && DockedTarget is Target_UI ui )
        {
            AudioSource.clip = SoundPlayer.Instance.GetClip( "Actuator" );
            AudioSource.Play();

            IsTransitioning = true;
            IsStanding = false;

            //----------------------------------------------
            rotationDirection = RotationDirection;
            accumulativeAngle = 0.0f;

            while ( accumulativeAngle < m_RotationAngle )
            {
                yield return new WaitForEndOfFrame();
                accumulativeAngle += m_DegreesPerSecondDown * Time.deltaTime;
                SetRotation( rotationDirection, accumulativeAngle );
            }

            SetRotation( rotationDirection, m_RotationAngle );

            //----------------------------------------------

            IsTransitioning = false;

            switch ( ui.ButtonType )
            {
                case Target_UI.UIButton.PLAY:
                    {
                        Destroy( ui.gameObject );
                        break;
                    }
                case Target_UI.UIButton.EXIT:
                    {
                        Destroy( ui.gameObject );
                        break;
                    }
            }

            IsStanding = false;
        }

        Target_UI pooledUITarget = null;

        // Attempt to get from pool.
        switch ( a_ButtonType )
        {
            case Target_UI.UIButton.PLAY:
                {
                    pooledUITarget = Instantiate( GalleryController.Instance.PersistantUIPlay ).GetComponent< Target_UI >();
                    break;
                }
            case Target_UI.UIButton.EXIT:
                {
                    pooledUITarget = Instantiate( GalleryController.Instance.PersistantUIExit ).GetComponent< Target_UI >();
                    break;
                }
        }

        // Set it to the docks position.
        SetRotation( RotationDirection, m_RotationAngle );
        pooledUITarget.transform.SetParent( DockPivot.transform );
        pooledUITarget.transform.localPosition = TargetAnchor.localPosition;
        pooledUITarget.transform.localRotation = TargetAnchor.localRotation;
        pooledUITarget.transform.localScale = TargetAnchor.localScale;
        pooledUITarget.gameObject.SetActive( true );
        DockedTarget = pooledUITarget;
        DockedTarget.TargetDock = this;

        // Play dock actuator sound.
        AudioSource.clip = SoundPlayer.Instance.GetClip( "Actuator" );
        AudioSource.Play();

        // Rotate new button back up.
        IsTransitioning = true;

        //----------------------------------------------
        rotationDirection = RotationDirection;
        accumulativeAngle = m_RotationAngle;

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
    }

    public void SpawnTarget( Target.TargetType a_TargetType, bool a_WithFlip, float a_FlipDownTimer = -1, bool a_NotifyOfKill = false )
    {
        if ( DockedTarget != null )
        {
            //DockedTarget.DestroyTarget();

            switch (DockedTarget.Type)
            {
                case Target.TargetType.WOOD:
                    {
                        GalleryController.Instance.PoolWoodBird.Despawn( DockedTarget as Target_WoodBird );
                        break;
                    }
                case Target.TargetType.FIRE:
                    {
                        GalleryController.Instance.PoolFireBird.Despawn( DockedTarget as Target_FireBird );
                        break;
                    }
                case Target.TargetType.GLASS:
                    {
                        GalleryController.Instance.PoolGlassBird.Despawn( DockedTarget as Target_GlassBird );
                        break;
                    }
            }

            DockedTarget = null;
            GalleryController.Instance.NotifyOfKill();
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
