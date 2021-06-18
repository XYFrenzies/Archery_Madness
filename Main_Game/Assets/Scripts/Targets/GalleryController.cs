using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class GalleryController : Singleton< GalleryController >
{
    public GameObject TargetPrefab_Wood;
    public GameObject TargetPrefab_Glass;
    public GameObject TargetPrefab_Fire;
    public GameObject ArrowBroad;
    public GameObject ArrowHammer;
    public GameObject ArrowWater;
    public GameObject TargetUIPrefab_Play;
    public GameObject TargetUIPrefab_Exit;

    public TargetDock UIDock;

    public GameObject PersistantGlassBird;
    public GameObject PersistantWoodBird;
    public GameObject PersistantFireBird;
    public GameObject PersistantUIPlay;
    public GameObject PersistantUIExit;
    public GameObject PersistantArrowBroad;
    public GameObject PersistantArrowHammer;
    public GameObject PersistantArrowWater;

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

        //m_Firebirds = new ObjectPool< Target_FireBird >( CreateFirebird, OnBirdActive, OnBirdInactive );
        //m_Glassbirds = new ObjectPool< Target_GlassBird >( CreateGlassbird, OnBirdActive, OnBirdInactive );
        //m_Woodbirds = new ObjectPool< Target_WoodBird >( CreateWoodbird, OnBirdActive, OnBirdInactive );
        //m_UIPlay = new ObjectPool< Target_UI >( CreatePlayUI, OnUIActive, OnUIInactive );
        //m_UIExit = new ObjectPool< Target_UI >( CreateExitUI, OnUIActive, OnUIInactive );

        //m_Firebirds.Populate( 10 );
        //m_Glassbirds.Populate( 10 );
        //m_Woodbirds.Populate( 10 );
        //m_UIPlay.Populate( 1 );
        //m_UIExit.Populate( 1 );

        PersistantGlassBird = Instantiate( TargetPrefab_Glass, new Vector3( 0, -10, 0 ), Quaternion.identity );
        PersistantWoodBird = Instantiate( TargetPrefab_Wood, new Vector3( 0, -10, 0 ), Quaternion.identity );
        PersistantFireBird = Instantiate( TargetPrefab_Fire, new Vector3( 0, -10, 0 ), Quaternion.identity );
        PersistantUIPlay = Instantiate( TargetUIPrefab_Play, new Vector3( 0, -10, 0 ), Quaternion.identity );
        PersistantUIExit = Instantiate( TargetUIPrefab_Exit, new Vector3( 0, -10, 0 ), Quaternion.identity );
        PersistantArrowBroad = Instantiate( ArrowBroad, new Vector3( 0, -10, 0 ), Quaternion.identity );
        PersistantArrowHammer = Instantiate( ArrowHammer, new Vector3( 0, -10, 0 ), Quaternion.identity );
        PersistantArrowWater = Instantiate( ArrowWater, new Vector3( 0, -10, 0 ), Quaternion.identity );

        PersistantGlassBird.SetActive( false );
        PersistantWoodBird.SetActive( false );
        PersistantFireBird.SetActive( false );
        PersistantUIPlay.SetActive( false );
        PersistantUIExit.SetActive( false );
        PersistantArrowBroad.SetActive( false );
        PersistantArrowHammer.SetActive( false );
        PersistantArrowWater.SetActive( false );
    }

    private Target_FireBird CreateFirebird()
    {
        GameObject newObject = Instantiate( Instance.TargetPrefab_Fire, new Vector3( 0, -10, 0 ), Quaternion.identity );
        Target_FireBird targetFire = newObject.GetComponent< Target_FireBird >();
        targetFire.Pool = PoolFireBird;
        newObject.SetActive( false );

        return targetFire;
    }

    //private Target_GlassBird CreateGlassbird()
    //{
    //    GameObject newObject = Instantiate( Instance.TargetPrefab_Glass, new Vector3( 0, -10, 0 ), Quaternion.identity );
    //    Target_GlassBird targetGlass = newObject.GetComponent< Target_GlassBird >();
    //    targetGlass.Pool = PoolGlassBird;
    //    newObject.SetActive( false );

    //    return targetGlass;
    //}

    //private Target_WoodBird CreateWoodbird()
    //{
    //    GameObject newObject = Instantiate( Instance.TargetPrefab_Wood, new Vector3( 0, -10, 0 ), Quaternion.identity );
    //    Target_WoodBird targetWood = newObject.GetComponent< Target_WoodBird >();
    //    targetWood.Pool = PoolWoodBird;
    //    newObject.SetActive( false );

    //    return targetWood;
    //}

    //private Target_UI CreatePlayUI()
    //{
    //    GameObject newObject = Instantiate( Instance.TargetUIPrefab_Play, new Vector3( 0, -10, 0 ), Quaternion.identity );
    //    Target_UI targetUI = newObject.GetComponent< Target_UI >();
    //    targetUI.Pool = PoolPlayUI;
    //    targetUI.gameObject.SetActive( false );

    //    return targetUI;
    //}

    //private Target_UI CreateExitUI()
    //{
    //    GameObject newObject = Instantiate( Instance.TargetUIPrefab_Exit, new Vector3( 0, -10, 0 ), Quaternion.identity );
    //    Target_UI targetUI = newObject.GetComponent< Target_UI >();
    //    targetUI.Pool = PoolExitUI;
    //    targetUI.gameObject.SetActive( false );

    //    return targetUI;
    //}

    //private void OnBirdActive( Target a_Bird )
    //{
    //    a_Bird.gameObject.SetActive( true );
    //}

    //private void OnBirdInactive( Target a_Bird )
    //{
    //    a_Bird.transform.position = new Vector3( 0, -10, 0 );
    //    a_Bird.transform.rotation = Quaternion.identity;

    //    // Resets
    //    Collider collider = a_Bird.GetComponent< Collider >();
    //    collider.isTrigger = false;

    //    Rigidbody rigidbody = a_Bird.GetComponent< Rigidbody >();
    //    rigidbody.useGravity = true;
    //    rigidbody.isKinematic = true;
    //}

    //private static void OnUIActive( Target_UI a_UI )
    //{
    //    a_UI.gameObject.SetActive( true );
    //}

    //private static void OnUIInactive( Target_UI a_UI )
    //{
    //    a_UI.transform.position = new Vector3( 0, -10, 0 );
    //    a_UI.transform.rotation = Quaternion.identity;

    //    // Resets
    //    Collider collider = a_UI.GetComponent< Collider >();
    //    collider.isTrigger = false;

    //    Rigidbody rigidbody = a_UI.GetComponent< Rigidbody >();
    //    rigidbody.useGravity = true;
    //    rigidbody.isKinematic = true;
    //}

    //------NEW-----------------------------------------------------

    public IEnumerator New_BeginSpawning()
    {
        EnableTrackMovement();
        SetMovementSpeeds( 0.5f );

        while ( GameStateManager.Instance.InGame )
        {
            yield return new WaitForSeconds( m_RespawnLoopTimer );
            New_SpawnNewTarget();
        }

        DestroyAllTargets();
        yield return SlowDownAndStop();
    }

    public bool New_SpawnNewTarget()
    {
        Target.TargetType newTargetType = ( Target.TargetType )UnityEngine.Random.Range( 2, 5 );

        TargetDock emptyDock = New_FindEmptyDock();

        if ( emptyDock == null )
        {
            return false;
        }

        StartCoroutine( emptyDock.New_ShowTarget( newTargetType, m_FlipDownTimer ) );
        return true;
    }

    public TargetDock New_FindEmptyDock()
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

    public IEnumerator New_RaisePlay()
    {
        yield return UIDock.New_ShowUI( Target_UI.UIButton.PLAY );
    }

    public IEnumerator New_RaiseExit()
    {
        yield return UIDock.New_ShowUI( Target_UI.UIButton.EXIT );
    }

    //---------------------------------------------------------------

    public void NotifyOfKill()
    {
        --m_ActiveTargets;
    }

    public void NotifyOfRespawn()
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

            SpawnRandomNewTarget( m_FlipDownTimer );
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
    [ SerializeField ] [ Range( 0.1f, 10.0f ) ] private float m_FlipDownTimer;
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
