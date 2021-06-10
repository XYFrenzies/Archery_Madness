using UnityEngine;
using System;
using System.Collections;

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

    public bool IsMenuMoving
    {
        get
        {
            return m_IsMenuMoving;
        }
    }

    private void Awake()
    {
        GameObject targetsGameObject = GameObject.Find( "Targets" );
        m_TargetDocksEnemy = targetsGameObject.GetComponentsInChildren< TargetDock >();

        foreach ( TargetDock targetDock in m_TargetDocksEnemy )
        {
            targetDock.TriggerSpawnTargetWithFlip( ( Target.TargetType )UnityEngine.Random.Range( 2, 5 ) );
        }
    }

    public void TriggerRaiseMenu()
    {
        if ( m_IsMenuRaised )
        {
            return;
        }

        StartCoroutine( RaiseMenu() );
    }

    public void TriggerLowerMenu()
    {
        if ( !m_IsMenuRaised )
        {
            return;
        }

        StartCoroutine( LowerMenu() );
    }

    private IEnumerator RaiseMenu()
    {
        m_IsMenuMoving = true;

        m_TargetDock_UI_Tutorial.TrackDock.IsActive = true;
        m_TargetDock_UI_Play.TrackDock.IsActive = true;
        m_TargetDock_UI_Endless.TrackDock.IsActive = true;

        Func< bool > allRaised = () => ( m_TargetDock_UI_Tutorial.TrackDock.CurrentIndex == 1 ) &&
                                       ( m_TargetDock_UI_Play.TrackDock.CurrentIndex == 1 ) && 
                                       ( m_TargetDock_UI_Endless.TrackDock.CurrentIndex == 1 );

        yield return new WaitUntil( allRaised );

        m_IsMenuRaised = true;
        m_IsMenuMoving = false;
    }

    private IEnumerator LowerMenu()
    {
        m_IsMenuMoving = true;

        m_TargetDock_UI_Tutorial.TrackDock.IsActive = true;
        m_TargetDock_UI_Play.TrackDock.IsActive = true;
        m_TargetDock_UI_Endless.TrackDock.IsActive = true;

        Func< bool > allLowered = () => ( m_TargetDock_UI_Tutorial.TrackDock.CurrentIndex == 0 ) &&
                                       ( m_TargetDock_UI_Play.TrackDock.CurrentIndex == 0 ) && 
                                       ( m_TargetDock_UI_Endless.TrackDock.CurrentIndex == 0 );

        yield return new WaitUntil( allLowered );

        m_IsMenuRaised = false;
        m_IsMenuMoving = false;
    }
    
    #pragma warning disable 0649

    [ SerializeField ] private TargetDock[] m_TargetDocksEnemy;
    [ SerializeField ] private TargetDock m_TargetDock_UI_Tutorial;
    [ SerializeField ] private TargetDock m_TargetDock_UI_Play;
    [ SerializeField ] private TargetDock m_TargetDock_UI_Endless;
    [ SerializeField ] private TargetDock m_TargetDock_UI_ExitTutorial;
    [ SerializeField ] private TargetDock m_TargetDock_UI_ExitPlay;
    [ SerializeField ] private TargetDock m_TargetDock_UI_ExitEndless;

    private bool m_IsMenuRaised;
    private bool m_IsMenuMoving;

    #pragma warning restore

}
