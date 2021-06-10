using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryController : Singleton< GalleryController >
{
    public GameObject Target_UI;
    public GameObject Target_Wood;
    public GameObject Target_Glass;
    public GameObject Target_Fire;

    public TargetDock[] TargetDocksEnemy
    {
        get
        {
            return m_TargetDocksEnemy;
        }
    }

    public TargetDock UI_PlayChallenge
    {
        get
        {
            return m_TargetDock_UI_PlayChallenge;
        }
    }

    public TargetDock UI_PlayTutorial
    {
        get
        {
            return m_TargetDock_UI_PlayTutorial;
        }
    }

    public TargetDock UI_ExitChallenge
    {
        get
        {
            return m_TargetDock_UI_ExitChallenge;
        }
    }

    public TargetDock UI_ExitTutorial
    {
        get
        {
            return m_TargetDock_UI_ExitTutorial;
        }
    }

    public TargetDock UI_ExitGame
    {
        get
        {
            return m_TargetDock_UI_ExitGame;
        }
    }
    
    [ SerializeField ] private TargetDock[] m_TargetDocksEnemy;
    [ SerializeField ] private TargetDock m_TargetDock_UI_PlayChallenge;
    [ SerializeField ] private TargetDock m_TargetDock_UI_PlayTutorial;
    [ SerializeField ] private TargetDock m_TargetDock_UI_ExitChallenge;
    [ SerializeField ] private TargetDock m_TargetDock_UI_ExitTutorial;
    [ SerializeField ] private TargetDock m_TargetDock_UI_ExitGame;

}
