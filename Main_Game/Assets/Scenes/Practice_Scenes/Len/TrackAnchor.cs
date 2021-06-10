using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackAnchor : MonoBehaviour
{
    public enum Type
    {
        LOOP,
        PING_PONG
    }

    public enum Mode
    {
        HALTING,
        CONTINUOUS
    }

    public enum Direction
    {
        FORWARD,
        REVERSE
    }

    public Transform[] AnchorPoints;
    public Type TrackingType
    {
        get
        {
            return m_TrackingType;
        }
    }

    public Mode TrackingMode
    {
        get
        {
            return m_TrackingMode;
        }
        set
        {
            m_TrackingMode = value;
        }
    }
    public Direction TrackingDirection
    {
        get
        {
            return m_TrackingDirection;
        }
        set
        {
            m_TrackingDirection = value;
        }
    }
    public float Speed
    {
        get
        {
            return m_Speed;
        }
        set
        {
            m_Speed = Mathf.Clamp( value, 0.0f, m_MaxSpeed );
        }
    }
    public bool IsActive
    {
        get
        {
            return m_IsActive;
        }
        set
        {
            m_IsActive = value;
        }
    }

    private void Awake()
    {
        m_ComingFrom = Mathf.Clamp( m_ComingFrom, 0, AnchorPoints.Length - 1 );
        m_GoingTo = GetNextIndex( m_ComingFrom );
        SetMoveDirection();
    }

    private void Update()
    {
        if ( AnchorPoints == null || AnchorPoints.Length == 1 )
        {
            return;
        }

        if ( m_IsActive )
        {
            Move();

            if ( HasPassedNextIndex() )
            {
                SetNextIndex();
            }
        }
    }

    private void Move()
    {
        transform.position += Time.deltaTime * m_Speed * m_MoveDirection;
    }

    private int GetNextIndex( int a_From )
    {
        int nextIndex = m_ComingFrom + ( m_TrackingDirection == Direction.FORWARD ? 1 : -1 );

        switch ( m_TrackingType )
        {
            case Type.LOOP:
                {
                    if ( nextIndex < 0 )
                    {
                        nextIndex = AnchorPoints.Length - 1;
                    }
                    else if ( nextIndex >= AnchorPoints.Length )
                    {
                        nextIndex = 0;
                    }

                    break;
                }
            case Type.PING_PONG:
                {
                    if ( nextIndex < 0 )
                    {
                        nextIndex = 1;
                        m_TrackingDirection = Direction.FORWARD;
                    }
                    else if ( nextIndex >= AnchorPoints.Length )
                    {
                        nextIndex = AnchorPoints.Length - 2;
                        m_TrackingDirection = Direction.REVERSE;
                    }

                    break;
                }
        }

        return nextIndex;
    }

    private void SetNextIndex()
    {
        int nextIndex = m_GoingTo + ( m_TrackingDirection == Direction.FORWARD ? 1 : -1 );

        switch ( m_TrackingType )
        {
            case Type.LOOP:
                {
                    if ( nextIndex < 0 )
                    {
                        nextIndex = AnchorPoints.Length - 1;
                        IsActive = m_TrackingMode == Mode.CONTINUOUS;
                    }
                    else if ( nextIndex >= AnchorPoints.Length )
                    {
                        nextIndex = 0;
                        IsActive = m_TrackingMode == Mode.CONTINUOUS;
                    }

                    break;
                }
            case Type.PING_PONG:
                {
                    if ( nextIndex < 0 )
                    {
                        nextIndex = 1;
                        m_TrackingDirection = Direction.FORWARD;
                        IsActive = m_TrackingMode == Mode.CONTINUOUS;
                    }
                    else if ( nextIndex >= AnchorPoints.Length )
                    {
                        nextIndex = AnchorPoints.Length - 2;
                        m_TrackingDirection = Direction.REVERSE;
                        IsActive = m_TrackingMode == Mode.CONTINUOUS;
                    }

                    break;
                }
        }

        m_ComingFrom = m_GoingTo;
        m_GoingTo = nextIndex;
        SetMoveDirection();
    }

    public void SetMoveDirection()
    {
        m_MoveDirection = Vector3.Normalize( AnchorPoints[ m_GoingTo ].position - 
                                             AnchorPoints[ m_ComingFrom ].position );
    }

    private bool HasPassedNextIndex()
    {
        Vector3 directionToNextIndex = Vector3.Normalize( AnchorPoints[ m_GoingTo ].position - transform.position );
        return Vector3.Dot( directionToNextIndex, m_MoveDirection ) <= 0;
    }

    [ SerializeField ] private Type m_TrackingType;
    [ SerializeField ] private Mode m_TrackingMode;
    [ SerializeField ] private Direction m_TrackingDirection;
    [ SerializeField ] private bool m_IsActive;
    [ SerializeField ] [ Range( 0.0f, m_MaxSpeed ) ] private float m_Speed;
    [ SerializeField ] private int m_ComingFrom;
    private int m_GoingTo;
    private const float m_MaxSpeed = 5.0f;
    
    private Vector3 m_MoveDirection;
}
