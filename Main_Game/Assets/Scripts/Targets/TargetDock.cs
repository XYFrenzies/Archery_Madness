using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Target DockedTarget { get; private set; }
    public TrackDock TrackDock { get; private set; }
    public Direction RotationDirection
    {
        get
        {
            return m_RotationDirection;
        }
    }
    public bool IsStanding { get; private set; } = true;
    public bool IsTransitioning { get; private set; } = false;

    private void Awake()
    {
        m_DegreesPerSecondDown = m_RotationAngle / m_TransitionTimeDown;
        m_DegreesPerSecondUp = m_RotationAngle / m_TransitionTimeUp;
        TrackDock = GetComponent< TrackDock >();
        IsStanding = m_IsInitiallyStanding;
        m_PersistantRotation = new Quaternion();
    }

    public void TriggerSpawnTargetWithFlip( Target.TargetType a_TargetType )
    {
        if ( DockedTarget != null || 
             a_TargetType == Target.TargetType.NONE || 
             a_TargetType == Target.TargetType.UI )
        {
            return;
        }

        StartCoroutine( SpawnTargetWithFlip( a_TargetType ) );
    }

    public void TriggerSpawnUITargetWithFlip( Target_UI.UIButton a_UIButtonType )
    {
        if ( DockedTarget != null )
        {
            return;
        }

        StartCoroutine( SpawnUITargetWithFlip( a_UIButtonType ) );
    }

    public void TriggerDestroyTargetWithFlip()
    {
        if ( DockedTarget == null )
        {
            return;
        }

        StartCoroutine( DestroyTargetWithFlip() );
    }

    public void TriggerFlipDown()
    {
        if ( IsTransitioning || !IsStanding )
        {
            return;
        }

        StartCoroutine( FlipDown() );
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

    private IEnumerator SpawnTargetWithFlip( Target.TargetType a_TargetType )
    {
        GameObject targetToCreate = null;

        switch ( a_TargetType )
        {
            case Target.TargetType.WOOD:
                {
                    targetToCreate = GalleryController.Instance.TargetPrefab_Wood;
                    break;
                }
            case Target.TargetType.FIRE:
                {
                    targetToCreate = GalleryController.Instance.TargetPrefabs_Fire;
                    break;
                }
            case Target.TargetType.GLASS:
                {
                    targetToCreate = GalleryController.Instance.TargetPrefab_Glass;
                    break;
                }
        }

        GameObject newTarget = Instantiate( targetToCreate, DockPivot.transform );
        newTarget.transform.localPosition = TargetAnchor.localPosition;
        DockedTarget = newTarget.GetComponentInChildren< Target >();
        DockedTarget.TargetDock = this;
        
        if ( !IsStanding )
        {
            yield return FlipUp();
        }
    }

    private IEnumerator SpawnUITargetWithFlip( Target_UI.UIButton a_ButtonType )
    {
        GameObject targetToCreate = null;

        switch ( a_ButtonType )
        {
            case Target_UI.UIButton.TUTORIAL:
                {
                    targetToCreate = GalleryController.Instance.TargetUIPrefab_Tutorial;
                    break;
                }
            case Target_UI.UIButton.PLAY:
                {
                    targetToCreate = GalleryController.Instance.TargetUIPrefab_Play;
                    break;
                }
            case Target_UI.UIButton.ENDLESS:
                {
                    targetToCreate = GalleryController.Instance.TargetUIPrefab_Endless;
                    break;
                }
            case Target_UI.UIButton.EXIT_TUTORIAL:
                {
                    targetToCreate = GalleryController.Instance.TargetUIPrefab_ExitTutorial;
                    break;
                }
            case Target_UI.UIButton.EXIT_PLAY:
                {
                    targetToCreate = GalleryController.Instance.TargetUIPrefab_ExitPlay;
                    break;
                }
            case Target_UI.UIButton.EXIT_ENDLESS:
                {
                    targetToCreate = GalleryController.Instance.TargetUIPrefab_ExitEndless;
                    break;
                }
        }

        GameObject newTarget = Instantiate( targetToCreate, DockPivot.transform );
        newTarget.transform.localPosition = TargetAnchor.localPosition;
        DockedTarget = newTarget.GetComponentInChildren< Target >();
        DockedTarget.TargetDock = this;
        
        if ( !IsStanding )
        {
            yield return FlipUp();
        }
    }

    private IEnumerator DestroyTargetWithFlip()
    {
        Destroy( DockedTarget.gameObject );
        DockedTarget = null;

        if ( IsStanding )
        {
            yield return FlipDown();
        }
    }

    private IEnumerator FlipDown()
    {
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

    private IEnumerator FlipUp()
    {
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
    private float m_DegreesPerSecondDown;
    private float m_DegreesPerSecondUp;
    private Quaternion m_PersistantRotation;
    private const float m_MaxRotationAngle = 120.0f;
    private const float m_MaxTransitionTime = 10.0f;

    #pragma warning restore
}
