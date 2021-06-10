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
        m_Wait = new WaitForEndOfFrame();
    }

    public void TriggerSpawnTarget( Target.TargetType a_TargetType )
    {
        if ( DockedTarget != null || a_TargetType == Target.TargetType.NONE )
        {
            return;
        }

        StartCoroutine( SpawnTarget( a_TargetType ) );
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

    private IEnumerator SpawnTarget( Target.TargetType a_TargetType )
    {
        if ( !IsStanding )
        {
            yield return FlipUp();
        }

        GameObject targetToCreate = null;

        switch ( a_TargetType )
        {
            case Target.TargetType.UI:
                {
                    targetToCreate = GalleryController.Instance.Target_UI;
                    break;
                }
            case Target.TargetType.WOOD:
                {
                    targetToCreate = GalleryController.Instance.Target_Wood;
                    break;
                }
            case Target.TargetType.FIRE:
                {
                    targetToCreate = GalleryController.Instance.Target_Fire;
                    break;
                }
            case Target.TargetType.GLASS:
                {
                    targetToCreate = GalleryController.Instance.Target_Glass;
                    break;
                }
        }

        DockedTarget = Instantiate( targetToCreate, TargetAnchor.position, TargetAnchor.rotation ).GetComponent< Target >();
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
            yield return m_Wait;
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
            yield return m_Wait;
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
                        DockPivot.rotation = Quaternion.AngleAxis( a_Angle, Vector3.right );
                        break;
                    }
                case Direction.Y_POS:
                    {
                        DockPivot.rotation = Quaternion.AngleAxis( a_Angle, Vector3.up );
                        break;
                    }
                case Direction.Z_POS:
                    {
                        DockPivot.rotation = Quaternion.AngleAxis( a_Angle, Vector3.forward );
                        break;
                    }
                case Direction.X_NEG:
                    {
                        DockPivot.rotation = Quaternion.AngleAxis( a_Angle, Vector3.left );
                        break;
                    }
                case Direction.Y_NEG:
                    {
                        DockPivot.rotation = Quaternion.AngleAxis( a_Angle, Vector3.down );
                        break;
                    }
                case Direction.Z_NEG:
                    {
                        DockPivot.rotation = Quaternion.AngleAxis( a_Angle, Vector3.back );
                        break;
                    }
            }
    }

    [ SerializeField ] [ Range( 0.0f, m_MaxRotationAngle ) ] private float m_RotationAngle = 90.0f;
    [ SerializeField ] [ Range( 0.1f, m_MaxTransitionTime ) ] private float m_TransitionTimeDown = 2.0f;
    [ SerializeField ] [ Range( 0.1f, m_MaxTransitionTime ) ] private float m_TransitionTimeUp = 2.0f; 
    [ SerializeField ] private Direction m_RotationDirection;
    private float m_DegreesPerSecondDown;
    private float m_DegreesPerSecondUp;
    private WaitForEndOfFrame m_Wait;
    private const float m_MaxRotationAngle = 120.0f;
    private const float m_MaxTransitionTime = 10.0f;
}
