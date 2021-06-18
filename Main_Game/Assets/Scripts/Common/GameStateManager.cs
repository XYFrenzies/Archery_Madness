using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.XR;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[ RequireComponent( typeof( SoundPlayer ) ) ]
[ RequireComponent( typeof( GalleryController ) ) ]
[ RequireComponent( typeof( TimeScoreController ) ) ]
public class GameStateManager : Singleton< GameStateManager >
{
    public HandController LeftController;
    public HandController RightController;
    public TimeController TimeController;
    public Quiver QuiverHammerhead;
    public Quiver QuiverBroadhead;
    public Quiver QuiverWaterhead;
    public Bow Bow;
    public BowDockController BowDockController;
    public Light DirectionalLight;

    public bool InGame
    {
        get
        {
            return m_InGame;
        }
        set
        {
            m_InGame = value;
        }
    }


    public TimeScoreController ScoreController { get; private set; }

    private void Start()
    {
        m_States = new Dictionary< GameState.State, GameState >();
        m_States.Add( GameState.State.MENU, new GameState_Menu() );
        m_States.Add( GameState.State.SCORE, new GameState_Score() );

        m_States.Add( GameState.State.PRE_TUTORIAL, new GameState_PreTutorial() );
        m_States.Add( GameState.State.TUTORIAL, new GameState_Tutorial() );
        m_States.Add( GameState.State.POST_TUTORIAL, new GameState_PostTutorial() );

        m_States.Add( GameState.State.PRE_PLAY, new GameState_PrePlay() );
        m_States.Add( GameState.State.PLAY, new GameState_Play() );
        m_States.Add( GameState.State.POST_PLAY, new GameState_PostPlay() );

        m_States.Add( GameState.State.PRE_ENDLESS, new GameState_PreEndless() );
        m_States.Add( GameState.State.ENDLESS, new GameState_Endless() );
        m_States.Add( GameState.State.POST_ENDLESS, new GameState_PostEndless() );

        m_CurrentState = GameState.State.MENU;

        TimeController = new TimeController();
        ScoreController = GetComponent< TimeScoreController >();

        DontDestroyOnLoad( this );
        DontDestroyOnLoad( ScoreController );
        DontDestroyOnLoad( GalleryController.Instance );

        m_MainCamera = SoundPlayer.Instance.GetSource( "Main Camera" );
        TimeController.OnSecondElapsed.AddListener( SpeedUp );
        TimeController.OnTimeUp.AddListener( OnTimeUp );

        //----OnPlay----
        m_OnPlay = new Task( GalleryController.Instance.New_RaiseExit );
        Task resetScore = new Task( ResetScore );
        Task turnOnLights3 = new Task( TurnOnLight );
        Task playIntroDialougue = new Task( PlayIntroDialogue );
        Task startTimer = new Task( StartTimer );
        Task triggerSpawning = new Task( GalleryController.Instance.New_BeginSpawning );

        m_OnPlay.OnComplete = resetScore;
        resetScore.OnComplete = turnOnLights3;
        turnOnLights3.OnComplete = playIntroDialougue;
        playIntroDialougue.OnComplete = startTimer;
        startTimer.OnComplete = triggerSpawning;

        //----OnExit----
        m_OnExit = new Task( StopSpawning );
        Task lowerExit1 = new Task( GalleryController.Instance.New_RaisePlay );
        Task outroDialogue1 = new Task( PlayOutroDialogue );
        Task resetRange1 = new Task( EndGame );
        Task turnOffLights2 = new Task( TurnOffLight );

        m_OnExit.OnComplete = lowerExit1;
        lowerExit1.OnComplete = outroDialogue1;
        outroDialogue1.OnComplete = resetRange1;
        resetRange1.OnComplete = turnOffLights2;

        //---OnTimeUp---
        m_OnTimeUp = new Task( StopSpawning );
        Task lowerExit2 = new Task( GalleryController.Instance.New_RaisePlay );
        Task outroDialogue2 = new Task( PlayOutroDialogue );
        Task resetRange2 = new Task( EndGame );
        Task turnOffLights3 = new Task( TurnOffLight );

        m_OnTimeUp.OnComplete = lowerExit2;
        lowerExit2.OnComplete = outroDialogue2;
        outroDialogue2.OnComplete = resetRange2;
        resetRange2.OnComplete = turnOffLights3;

        StartCoroutine( PlayIntroSounds() );
    }
    
    //-----------------------Callbacks------------------------

    public IEnumerator PlayIntroSounds()
    {
        yield return GalleryController.Instance.New_RaisePlay();
        SoundPlayer.Instance.Play( "PlayerInvitation", "Barker", 1.0f, true );
        MusicManager.Instance.PlayMenuMusic();
    }

    public IEnumerator PlayIntroDialogue()
    {
        SoundPlayer.Instance.Play( "EvilBirdReminder", "Barker", 1.0f, true );
        yield return null;
    }

    public IEnumerator PlayOutroDialogue()
    {
        if ( ScoreController.CurrentScore.Value < 50 )
        {
            BarkerDialogueController.Instance.TriggerBarker( BarkerDialogueController.BarkerDialogue.LowScore );
        }
        else
        {
            BarkerDialogueController.Instance.TriggerBarker( BarkerDialogueController.BarkerDialogue.HighScore );
        }
        yield return null;
    }

    public IEnumerator EndGame()
    {
        MusicManager.Instance.PlayMenuMusic();
        m_InGame = false;
        TimeController.StopTimer();
        yield return null;
    }

    public IEnumerator ResetScore()
    {
        ScoreController.ResetCurrentScore();
        yield return null;
    }

    private void SpeedUp()
    {
        GalleryController.Instance.SetMovementSpeeds( TimeController.Progression * 1.5f + 0.5f );
    }

    public void OnTimeUp()
    {
        m_OnTimeUp.Trigger();
    }

    public IEnumerator StopSpawning()
    {
        m_InGame = false;
        yield return null;
    }

    public IEnumerator TurnOnLight()
    {
        DirectionalLight.enabled = true;
        yield return null;
    }

    public IEnumerator TurnOffLight()
    {
        DirectionalLight.enabled = false;
        yield return null;
    }

    public GameState.State Current
    {
        get
        {
            return m_CurrentState;
        }
        set
        {
            m_CurrentState = value;
        }
    }

    public IEnumerator OnKill()
    {
        GalleryController.Instance.UIDock.DestroyTarget();
        yield return new WaitForEndOfFrame();
    }

    public void OnPlay()
    {
        m_OnPlay.Trigger();
    }

    public void OnExit()
    {
        m_OnExit.Trigger();
    }

    public IEnumerator StartTimer()
    {
        yield return TimeScoreController.Instance.TurnOnDisplay( 0, 2, 0 );
        MusicManager.Instance.PlayGamePlayMusic();
        m_InGame = true;
        TimeController.StartTimer( 2, 0 );
        yield return new WaitForSeconds( 0.1f );
    }

    public IEnumerator StopTimer()
    {
        TimeController.StopTimer();
        m_InGame = false;
        yield return new WaitForSeconds( 1.0f );
    }

    // Called when Bow is picked up.
    public void OnBowPickup( BaseInteractionEventArgs a_Args )
    {
        Bow.SetPhysics( false );

        BowDockController.StopRedockTimer();
    }

    // Called when Bow is dropped.
    public void OnBowDrop( BaseInteractionEventArgs a_Args )
    {
        Bow.SetPhysics( true );
        
        BowDockController.StartRedockTimer();
    }

    // Called when arrow makes collides with something.
    public void RegisterShot( ContactScenario a_ContactScenario )
    {
        if ( m_InGame )
        {
            TimeScoreController.Instance.AddToScore( a_ContactScenario.ResultantScore() );
            TimeController.RegisterShotFired();
        }
        
        // Called when arrow hits something.
        if ( a_ContactScenario.HitTarget )
        {
            // If target was not a specific type.
            if ( a_ContactScenario.Target.Type == Target.TargetType.NONE )
            {
                // Do nothing
            }

            // If target was UI element.
            else if ( a_ContactScenario.Target.Type == Target.TargetType.UI )
            {
                Target_UI button = a_ContactScenario.Target as Target_UI;

                switch ( button.ButtonType )
                {
                    case Target_UI.UIButton.PLAY:
                        {
                            OnPlay();
                            button.DestroyTarget();
                            break;
                        }
                    case Target_UI.UIButton.EXIT:
                        {
                            OnExit();
                            button.DestroyTarget();
                            break;
                        }
                }

                return;
            }

            // Not None or UI Element, must be a bird.
            else
            {
                if ( a_ContactScenario.HitCorrectTarget )
                {
                    a_ContactScenario.Target.DestroyTarget();
                    GalleryController.Instance.NotifyOfKill();
                    BarkerDialogueController.Instance.TriggerBarker( BarkerDialogueController.BarkerDialogue.PlayerHit );

                    switch (a_ContactScenario.Target.Type)
                    {
                        case Target.TargetType.WOOD:
                            {
                                SoundPlayer.Instance.PlayAtLocation( "BirdHit_Wood", a_ContactScenario.Target.transform.position, 1.0f );
                                break;
                            }
                        case Target.TargetType.FIRE:
                            {
                                SoundPlayer.Instance.PlayAtLocation( "BirdHit_Fire", a_ContactScenario.Target.transform.position, 1.0f );
                                break;
                            }
                        case Target.TargetType.GLASS:
                            {
                                SoundPlayer.Instance.PlayAtLocation( "BirdHit_Glass", a_ContactScenario.Target.transform.position, 1.0f );
                                break;
                            }
                    }
                }
                else
                {
                    if ( InGame )
                    // Animation broken, reenable on fix.
                    BarkerDialogueController.Instance.TriggerBarker( BarkerDialogueController.BarkerDialogue.PlayerMiss );
                }
            }
        }

        // No target hit of any kind.
        else
        {
            if ( InGame )
            // Animation broken, reenable on fix.
            BarkerDialogueController.Instance.TriggerBarker( BarkerDialogueController.BarkerDialogue.PlayerMiss );
        }

        a_ContactScenario.Arrow.DestroyArrow();
    }

    //-------------------------------------------------------------

    // Get State associated with the flag.
    public GameState GetState( GameState.State a_State )
    {
        return m_States[ a_State ];
    }

    private GameState.State m_CurrentState;
    private Dictionary< GameState.State, GameState > m_States;

    private Task m_OnPlay;
    private Task m_OnExit;
    private Task m_OnTimeUp;

    private bool m_InGame;
    private AudioSource m_MainCamera;

    //-------------------------------Game Flags---------------------------------------------------
    #pragma warning disable 0414

    #pragma warning restore 0414
}