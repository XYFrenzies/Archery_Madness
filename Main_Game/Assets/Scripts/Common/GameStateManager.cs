using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.XR;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[ RequireComponent( typeof( SoundPlayer ) ) ]
[ RequireComponent( typeof( GalleryController ) ) ]
[ RequireComponent( typeof( ScoreController ) ) ]
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


    public ScoreController ScoreController { get; private set; }

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
        ScoreController = GetComponent< ScoreController >();

        DontDestroyOnLoad( this );
        DontDestroyOnLoad( ScoreController );
        DontDestroyOnLoad( GalleryController.Instance );

        GetState( m_CurrentState ).Start();

        //----OnTutorial----
        
        //----OnTutorialExit----
        m_OnTutorialExit = new Task( GalleryController.Instance.UIDock_Middle.FlipDown );
        Task turnOffLights1;
        Task raiseMenu1 = new Task( GalleryController.Instance.RaiseMainMenu );
        m_OnTutorialExit.OnComplete = raiseMenu1;
        //----OnPlay----

        //----OnPlayExit----

        //----OnEndless----
        m_OnEndless = new Task( GalleryController.Instance.LowerMainMenu );
        Task raiseExit = new Task( GalleryController.Instance.RaiseEndlessExit );
        Task turnOnLights3 = new Task( TurnOnLight );
        Task playIntroDialougue = new Task( PlayGameStateDialogue );
        Task startTimer = new Task( StartTimer  );
        Task triggerSpawning = new Task( GalleryController.Instance.BeginEndlessSpawningRoutine );

        m_OnEndless.OnComplete = raiseExit;
        raiseExit.OnComplete = turnOnLights3;
        turnOnLights3.OnComplete = playIntroDialougue;
        playIntroDialougue.OnComplete = startTimer;
        startTimer.OnComplete = triggerSpawning;
        //----OnEndlessExit----
        m_OnEndlessExit = new Task( GalleryController.Instance.UIDock_Middle.FlipDown );
        Task turnOffLights3 = new Task( TurnOffLight );
        Task raiseMenu3 = new Task( GalleryController.Instance.RaiseMainMenu );
        m_OnEndlessExit.OnComplete = raiseMenu3;

        // Test stuff.
        
        //GalleryController.Instance.DisableTrackMovement();
        GalleryController.Instance.TriggerRaiseMenu( new Task( PlayIntroSounds ) );
        
        //OnEndless();
    }

    public IEnumerator PlayIntroSounds()
    {
        yield return new WaitForSeconds( 1.0f );
        SoundPlayer.Instance.Play( "PlayerInvitation", "Barker", 1.0f, true );
        SoundPlayer.Instance.PlayRepeat( "Music", 0.2f, true );
    }

    public IEnumerator PlayGameStateDialogue()
    {
        SoundPlayer.Instance.Play( "EvilBirdReminder", "Barker", 1.0f, true );
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

    //-----------------------Callbacks------------------------

    public IEnumerator OnKill()
    {
        GalleryController.Instance.UIDock_Middle.DestroyTarget();
        yield return new WaitForEndOfFrame();
    }

    public void OnTutorial()
    {
        m_OnTutorial.Trigger();
    }

    public void OnPlay()
    {

    }

    public void OnEndless()
    {
        m_OnEndless.Trigger();
    }

    public void OnExitTutorial()
    {

    }

    public void OnExitPlay()
    {

    }

    public void OnExitEndless()
    {

    }

    public IEnumerator StartTimer()
    {
        TimeController.StartTimer();
        yield return new WaitForSeconds( 1.0f );
    }

    public IEnumerator StopTimer()
    {
        TimeController.StopTimer();
        yield return new WaitForSeconds( 1.0f );
    }

    // Called when Bow is picked up.
    public void OnBowPickup( BaseInteractionEventArgs a_Args )
    {
        m_BowPickedUp = true;
        Bow.SetPhysics( false );

        BowDockController.StopRedockTimer();
    }

    // Called when Bow is dropped.
    public void OnBowDrop( BaseInteractionEventArgs a_Args )
    {
        m_BowPickedUp = false;
        Bow.SetPhysics( true );
        
        BowDockController.StartRedockTimer();
    }

    // Called when arrow makes collides with something.
    public void RegisterShot( ContactScenario a_ContactScenario )
    {
        // Called when arrow hits something.
        if ( a_ContactScenario.HitTarget )
        {
            // If target was not a specific type.
            if ( a_ContactScenario.Target.Type == Target.TargetType.NONE )
            {
                //a_ContactScenario.Arrow.DestroyArrow();
            }

            // If target was UI element.
            else if ( a_ContactScenario.Target.Type == Target.TargetType.UI )
            {
                Target_UI button = a_ContactScenario.Target as Target_UI;
                switch ( button.ButtonType )
                {
                    case Target_UI.UIButton.TUTORIAL:
                        {
                            OnTutorial();
                            button.DestroyTarget();
                            //a_ContactScenario.Arrow.DestroyArrow();
                            break;
                        }
                    case Target_UI.UIButton.PLAY:
                        {
                            OnPlay();
                            button.DestroyTarget();
                            //a_ContactScenario.Arrow.DestroyArrow();
                            break;
                        }
                    case Target_UI.UIButton.ENDLESS:
                        {
                            OnEndless();
                            button.DestroyTarget();
                            //a_ContactScenario.Arrow.DestroyArrow();
                            break;
                        }
                    case Target_UI.UIButton.EXIT_TUTORIAL:
                        {
                            OnExitTutorial();
                            button.DestroyTarget();
                            //a_ContactScenario.Arrow.DestroyArrow();
                            break;
                        }
                    case Target_UI.UIButton.EXIT_PLAY:
                        {
                            OnExitPlay();
                            button.DestroyTarget();
                            //a_ContactScenario.Arrow.DestroyArrow();
                            break;
                        }
                    case Target_UI.UIButton.EXIT_ENDLESS:
                        {
                            OnExitEndless();
                            button.DestroyTarget();
                            //a_ContactScenario.Arrow.DestroyArrow();
                            break;
                        }
                }

                return;
            }

            // Not None or UI Element, must be a bird.
            else
            {
                // Define score calculation in ContactScenario.ResultantScore()
                ScoreController.CurrentScore.Increment( a_ContactScenario.ResultantScore() );

                if ( a_ContactScenario.HitCorrectTarget )
                {
                    a_ContactScenario.Target.DestroyTarget();
                    GalleryController.Instance.NotifyOfKill();
                    SoundPlayer.Instance.Play( "OnHit", "Barker", 1.0f );

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
                    //a_ContactScenario.Arrow.DestroyArrow();
                    SoundPlayer.Instance.Play( "OnMiss", "Barker", 1.0f );
                }

                //a_ContactScenario.Arrow.DestroyArrow();
            }
        }

        // No target hit of any kind.
        else
        {
            //a_ContactScenario.Arrow.DestroyArrow();
            SoundPlayer.Instance.Play( "OnMiss", "Barker", 1.0f );
        }
    }

    //-------------------------------------------------------------

    // Get State associated with the flag.
    public GameState GetState( GameState.State a_State )
    {
        return m_States[ a_State ];
    }

    private GameState.State m_CurrentState;
    private Dictionary< GameState.State, GameState > m_States;

    private Task m_OnTutorial;
    private Task m_OnPlay;
    private Task m_OnEndless;
    private Task m_OnTutorialExit;
    private Task m_OnPlayExit;
    private Task m_OnEndlessExit;

    //-------------------------------Game Flags---------------------------------------------------
    #pragma warning disable 0414

    private bool m_BowPickedUp;

    #pragma warning restore 0414
}