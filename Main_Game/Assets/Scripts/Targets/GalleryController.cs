using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class GalleryController : Singleton< GalleryController >
{
    public GameObject TargetPrefab_Wood;
    public GameObject TargetPrefab_Glass;
    public GameObject TargetPrefabs_Fire;

    public GameObject TargetUIPrefab_Tutorial;
    public GameObject TargetUIPrefab_Play;
    public GameObject TargetUIPrefab_Endless;
    public GameObject TargetUIPrefab_ExitTutorial;
    public GameObject TargetUIPrefab_ExitPlay;
    public GameObject TargetUIPrefab_ExitEndless;

    public TargetDock UIDock_Left;
    public TargetDock UIDock_Middle;
    public TargetDock UIDock_Right;

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
        // This function is called when the player destroys an enemy target.
        --m_ActiveTargets;

        if ( m_ActiveTargets < m_PersistantCount )
        {
            SpawnRandomNewTarget();
        }
    }

    private void NotifyOfRespawn()
    {
        // This function is called when a target is spawned.
        ++m_ActiveTargets;
    }

    public void SetMovementSpeeds( float a_Speed )
    {
        foreach ( TargetDock birdDock in BirdTargetDocks )
        {
            birdDock.TrackDock.Speed = a_Speed;
        }
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

    public void TriggerTutorialSpawningRoutine()
    {
        if ( m_TutorialSpawningRoutine != null )
        {
            StopCoroutine( m_TutorialSpawningRoutine );
        }

        m_TutorialSpawningRoutine = StartCoroutine( BeginTutorialSpawningRoutine() );
    }

    public void TriggerPlaySpawningRoutine()
    {
        if ( m_PlaySpawningRoutine != null )
        {
            StopCoroutine( m_PlaySpawningRoutine );
        }

        m_PlaySpawningRoutine = StartCoroutine( BeginPlaySpawningRoutine() );
    }

    public void TriggerEndlessSpawningRoutine()
    {
        if ( m_EndlessSpawningRoutine != null )
        {
            StopCoroutine( m_EndlessSpawningRoutine );
        }

        m_EndlessSpawningRoutine = StartCoroutine( BeginEndlessSpawningRoutine() );
    }

    public IEnumerator BeginTutorialSpawningRoutine()
    {
        yield return null;
    }

    public IEnumerator BeginPlaySpawningRoutine()
    {
        yield return null;
    }

    public IEnumerator BeginEndlessSpawningRoutine()
    {
        EnableTrackMovement();

        while ( true )
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

    public void TriggerRaiseMenu( Task a_OnComplete = null )
    {
        StartCoroutine( RaiseMainMenu( a_OnComplete ) );
    }

    public void TriggerLowerMenu( Task a_OnComplete = null )
    {
        StartCoroutine( LowerMainMenu( a_OnComplete ) );
    }

    public IEnumerator RaiseMainMenu( Task a_OnComplete = null )
    {
        UIDock_Middle.SpawnUITarget(Target_UI.UIButton.ENDLESS, true );

        yield return new WaitUntil( () => !UIDock_Middle.IsTransitioning );

        if ( a_OnComplete != null )
        {
            a_OnComplete.Trigger();
        }
    }

    public IEnumerator RaiseMainMenu()
    {
        UIDock_Left.SpawnUITarget( Target_UI.UIButton.TUTORIAL, true );
        UIDock_Middle.SpawnUITarget(Target_UI.UIButton.PLAY, true );
        UIDock_Right.SpawnUITarget( Target_UI.UIButton.ENDLESS, true );

        yield return new WaitUntil( () => 
        !UIDock_Left.IsTransitioning && 
        !UIDock_Middle.IsTransitioning && 
        !UIDock_Right.IsTransitioning );
    }

    public IEnumerator LowerMainMenu( Task a_OnComplete = null )
    {
        UIDock_Left.TriggerFlipDown();
        UIDock_Middle.TriggerFlipDown();
        UIDock_Right.TriggerFlipDown();

        yield return new WaitUntil( () => 
        !UIDock_Left.IsTransitioning && 
        !UIDock_Middle.IsTransitioning && 
        !UIDock_Right.IsTransitioning );

        // Put things here to occur after menu lowered.
        if ( a_OnComplete != null )
        {
            a_OnComplete.Trigger();
        }
    }

    public IEnumerator LowerMainMenu()
    {
        UIDock_Middle.TriggerFlipDown();

        yield return new WaitUntil( () => !UIDock_Middle.IsTransitioning );
    }

    public IEnumerator RaiseTutorialExit()
    {
        UIDock_Middle.SpawnUITarget( Target_UI.UIButton.EXIT_TUTORIAL, true );

        yield return new WaitUntil( () => !UIDock_Middle.IsTransitioning );
    }

    public IEnumerator RaisePlayExit()
    {
        UIDock_Middle.SpawnUITarget( Target_UI.UIButton.EXIT_PLAY, true );

        yield return new WaitUntil( () => !UIDock_Middle.IsTransitioning );
    }

    public IEnumerator RaiseEndlessExit()
    {
        UIDock_Middle.SpawnUITarget( Target_UI.UIButton.EXIT_ENDLESS, true );

        yield return new WaitUntil( () => !UIDock_Middle.IsTransitioning );
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
    private Coroutine m_EndlessSpawningRoutine;
    private int m_ActiveTargets;

    #pragma warning restore

}
