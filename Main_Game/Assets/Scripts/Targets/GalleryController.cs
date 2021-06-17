using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class GalleryController : Singleton< GalleryController >
{
    public GameObject TargetPrefab_Wood;
    public GameObject TargetPrefab_Glass;
    public GameObject TargetPrefab_Fire;

    public GameObject TargetUIPrefab_Play;
    public GameObject TargetUIPrefab_Exit;

    public TargetDock UIDock;

    public ObjectPool< Target_FireBird > PoolFireBird
    {
        get
        {
            return m_Firebirds;
        }
    }

    public ObjectPool< Target_GlassBird > PoolGlassBird
    {
        get
        {
            return m_Glassbirds;
        }
    }

    public ObjectPool< Target_WoodBird > PoolWoodBird
    {
        get
        {
            return m_Woodbirds;
        }
    }

    public ObjectPool< Target_UI > PoolPlayUI
    {
        get
        {
            return m_UIPlay;
        }
    }

    public ObjectPool< Target_UI > PoolExitUI
    {
        get
        {
            return m_UIExit;
        }
    }
 
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

        m_Firebirds = new ObjectPool< Target_FireBird >( CreateFirebird, OnBirdActive, OnBirdInactive );
        m_Glassbirds = new ObjectPool< Target_GlassBird >( CreateGlassbird, OnBirdActive, OnBirdInactive );
        m_Woodbirds = new ObjectPool< Target_WoodBird >( CreateWoodbird, OnBirdActive, OnBirdInactive );
        m_UIPlay = new ObjectPool< Target_UI >( CreatePlayUI, OnUIActive, OnUIInactive );
        m_UIExit = new ObjectPool< Target_UI >( CreateExitUI, OnUIActive, OnUIInactive );

        m_Firebirds.Populate( 10 );
        m_Glassbirds.Populate( 10 );
        m_Woodbirds.Populate( 10 );
        m_UIPlay.Populate( 1 );
        m_UIExit.Populate( 1 );
    }

    private static Target_FireBird CreateFirebird()
    {
        return Instantiate( Instance.TargetPrefab_Fire, new Vector3( 0, -10, 0 ), Quaternion.identity ).GetComponent< Target_FireBird >();
    }

    private static Target_GlassBird CreateGlassbird()
    {
        return Instantiate( Instance.TargetPrefab_Glass, new Vector3( 0, -10, 0 ), Quaternion.identity ).GetComponent< Target_GlassBird >();
    }

    private static Target_WoodBird CreateWoodbird()
    {
        return Instantiate( Instance.TargetPrefab_Wood, new Vector3( 0, -10, 0 ), Quaternion.identity ).GetComponent< Target_WoodBird >();
    }

    private static Target_UI CreatePlayUI()
    {
        return Instantiate( Instance.TargetUIPrefab_Play, new Vector3( 0, -10, 0 ), Quaternion.identity ).GetComponent< Target_UI >();
    }

    private static Target_UI CreateExitUI()
    {
        return Instantiate( Instance.TargetUIPrefab_Exit, new Vector3( 0, -10, 0 ), Quaternion.identity ).GetComponent< Target_UI >();
    }

    private static void OnBirdActive( Target a_Bird )
    {
        a_Bird.GetComponent< ShatterObject >().gameObject?.SetActive( true );
    }

    private static void OnBirdInactive( Target a_Bird )
    {
        a_Bird.gameObject.transform.position = new Vector3( 0, -10, 0 );
        //a_Bird.GetComponent< ShatterObject >().gameObject?.SetActive( false );
    }

    private static void OnUIActive( Target_UI a_UI )
    {
        a_UI.GetComponent< ShatterObject >().gameObject?.SetActive( true );
    }

    private static void OnUIInactive( Target_UI a_UI )
    {
        a_UI.gameObject.transform.position = new Vector3( 0, -10, 0 );
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

    // Needs change for object pooling
    public void DestroyAllTargets()
    {
        foreach ( TargetDock dock in BirdTargetDocks )
        {
            dock.TriggerFlipDownAndDestroy();
        }
    }

    public void TriggerSlowDown()
    {
        StartCoroutine( SlowDownAndStop() );
    }

    private IEnumerator SlowDownAndStop()
    {
        float timeLeft = 1.0f;
        float initialSpeed = m_AllMovementSpeeds;

        while ( timeLeft > 0.0f )
        {
            timeLeft -= Time.deltaTime;
            SetMovementSpeeds( initialSpeed * timeLeft );
            yield return null;
        }

        DisableTrackMovement();
    }

    public void TriggerSpawning()
    {
        if ( m_SpawningRoutine != null )
        {
            StopCoroutine( m_SpawningRoutine );
        }

        m_SpawningRoutine = StartCoroutine( BeginSpawning() );
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

        DestroyAllTargets();
        yield return SlowDownAndStop();
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

    private ObjectPool< Target_FireBird > m_Firebirds;
    private ObjectPool< Target_GlassBird > m_Glassbirds;
    private ObjectPool< Target_WoodBird > m_Woodbirds;
    private ObjectPool< Target_UI > m_UIPlay;
    private ObjectPool< Target_UI > m_UIExit;

    #pragma warning restore

}
