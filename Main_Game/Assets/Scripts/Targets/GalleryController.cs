using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class GalleryController : Singleton< GalleryController >
{
    public GameObject TargetPrefab_Wood;
    public GameObject TargetPrefab_Glass;
    public GameObject TargetPrefabs_Fire;

    public GameObject TargetUIPrefab_Play;
    public GameObject TargetUIPrefab_Exit;

    public TargetDock UIDock;

    public bool AllTargetsFilled
    {
        get
        {
            return Array.TrueForAll( BirdTargetDocks, targetDock => targetDock.DockedTarget );
        }
    }

    public int ActiveTargets
    {
        get
        {
            return m_ActiveTargets;
        }
    }

    public int InactiveTargets
    {
        get
        {
            return BirdTargetDocks.Length - m_ActiveTargets;
        }
    }

    public int TargetDockCount
    {
        get
        {
            return BirdTargetDocks.Length;
        }
    }

    private void Awake()
    {
        GameObject targetsGameObject = GameObject.Find( "Targets" );
        BirdTargetDocks = targetsGameObject.GetComponentsInChildren< TargetDock >();
    }

    public void NotifyOfKill()
    {
        --m_ActiveTargets;

        if ( m_ActiveTargets < m_PersistantCount && GameStateManager.Instance.InGame )
        {
            SpawnRandomNewTarget();
        }
    }

    private void NotifyOfRespawn()
    {
        ++m_ActiveTargets;
    }

    public void SetMovementSpeeds( float a_Speed )
    {
        foreach ( TargetDock birdDock in BirdTargetDocks )
        {
            birdDock.TrackDock.Speed = a_Speed;
        }
        m_AllMovementSpeeds = a_Speed;
    }

    public void EnableTrackMovement()
    {
        foreach ( TargetDock birdDock in BirdTargetDocks )
        {
            birdDock.TrackDock.IsActive = true;
        }
    }

    public void DisableTrackMovement()
    {
        foreach ( TargetDock birdDock in BirdTargetDocks )
        {
            birdDock.TrackDock.IsActive = false;
        }
    }

    public void DestroyAllTargets()
    {
        foreach ( TargetDock dock in BirdTargetDocks )
        {
            dock.TriggerFlipDownAndDestroy();
        }
    }

    public void TriggerSlowDown()
    {
        StartCoroutine( SlowDown() );
    }

    private IEnumerator SlowDown()
    {
        float timeLeft = 1.0f;
        float initialSpeed = m_AllMovementSpeeds;

        while ( timeLeft > 0.0f )
        {
            timeLeft -= Time.deltaTime;
            SetMovementSpeeds( initialSpeed * timeLeft );
            yield return null;
        }
    }

    public void TriggerSpawning()
    {
        if ( m_SpawningRoutine != null )
        {
            StopCoroutine( m_SpawningRoutine );
        }

        m_SpawningRoutine = StartCoroutine( BeginSpawning() );
    }

    public void TriggerStopSpawning()
    {
        if ( m_SpawningRoutine != null )
        {
            StopCoroutine( m_SpawningRoutine );
        }

        TriggerSlowDown();
        DestroyAllTargets();
    }

    public IEnumerator BeginSpawning()
    {
        EnableTrackMovement();
        SetMovementSpeeds( 0.5f );

        while ( GameStateManager.Instance.InGame )
        {
            yield return new WaitForSeconds( m_RespawnLoopTimer );

            SpawnRandomNewTarget( m_EndlessFlipDownTimer );
        }
    }

    private bool SpawnRandomNewTarget( float a_DespawnTimer = -1 )
    {
        Target.TargetType randomType = ( Target.TargetType )
            UnityEngine.Random.Range( 2, 5 );
        bool spawnedTarget = SpawnNewTarget( randomType, a_DespawnTimer );
        if ( spawnedTarget )
        {
            NotifyOfRespawn();
            return true;
        }

        return false;
    }

    private bool SpawnNewTarget( Target.TargetType a_TargetType, float a_DespawnTimer = -1 )
    {
        TargetDock emptyDock = FindRandomEmptyTargetDock();
        
        if ( emptyDock == null )
        {
            return false;
        }

        emptyDock.SpawnTarget( a_TargetType, true, a_DespawnTimer, true );
        return true;
    }

    public void TriggerRaisePlay( Task a_OnComplete = null )
    {
        StartCoroutine( RaisePlay( a_OnComplete ) );
    }

    public void TriggerLowerPlay( Task a_OnComplete = null )
    {
        StartCoroutine( LowerPlay( a_OnComplete ) );
    }

    public IEnumerator RaisePlay( Task a_OnComplete = null )
    {
        UIDock.SpawnUITarget(Target_UI.UIButton.PLAY, true );

        yield return new WaitUntil( () => !UIDock.IsTransitioning );

        if ( a_OnComplete != null )
        {
            a_OnComplete.Trigger();
        }
    }

    public IEnumerator RaisePlay()
    {
        UIDock.SpawnUITarget(Target_UI.UIButton.PLAY, true );

        yield return new WaitUntil( () => !UIDock.IsTransitioning );
    }

    public IEnumerator LowerPlay( Task a_OnComplete = null )
    {
        UIDock.TriggerFlipDown();

        yield return new WaitUntil( () => !UIDock.IsTransitioning );

        // Put things here to occur after menu lowered.
        if ( a_OnComplete != null )
        {
            a_OnComplete.Trigger();
        }
    }

    public IEnumerator LowerPlay()
    {
        UIDock.TriggerFlipDown();

        yield return new WaitUntil( () => !UIDock.IsTransitioning );
    }

    public IEnumerator RaiseExit()
    {
        UIDock.SpawnUITarget( Target_UI.UIButton.EXIT, true );

        yield return new WaitUntil( () => !UIDock.IsTransitioning );
    }

    private TargetDock FindRandomEmptyTargetDock()
    {
        if ( InactiveTargets == 0 )
        {
            return null;
        }

        int index = UnityEngine.Random.Range( 0, InactiveTargets );
        int currentIndex = 0;

        foreach ( TargetDock targetDock in BirdTargetDocks )
        {
            if ( targetDock.DockedTarget == null )
            {
                if ( currentIndex == index )
                {
                    return targetDock;
                }
                else
                {
                    ++currentIndex;
                }
            }
        }

        return null;
    }
    
    #pragma warning disable 0649

    [ SerializeField ] private TargetDock[] BirdTargetDocks;

    [ SerializeField ] [ Range( 0, 23 ) ] private int m_PersistantCount;
    [ SerializeField ] [ Range( 0.1f, 10.0f ) ] private float m_RespawnLoopTimer;
    [ SerializeField ] [ Range( 0.1f, 10.0f ) ] private float m_EndlessFlipDownTimer;
    private Coroutine m_TutorialSpawningRoutine;
    private Coroutine m_PlaySpawningRoutine;
    private Coroutine m_SpawningRoutine;
    private int m_ActiveTargets;
    private float m_AllMovementSpeeds;

    #pragma warning restore

}
