using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.XR;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[ RequireComponent( typeof( DestructionController ) ) ]
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
    public GalleryController GalleryController;

    public ScoreController ScoreController { get; private set; }

    private void Awake()
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
        DontDestroyOnLoad( GalleryController );

        GetState( m_CurrentState ).Start();
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

    public void OnTutorial()
    {

    }

    public void OnPlay()
    {

    }

    public void OnEndless()
    {

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
                            break;
                        }
                    case Target_UI.UIButton.PLAY:
                        {
                            OnPlay();
                            break;
                        }
                    case Target_UI.UIButton.ENDLESS:
                        {
                            OnEndless();
                            break;
                        }
                    case Target_UI.UIButton.EXIT_TUTORIAL:
                        {
                            OnExitTutorial();
                            break;
                        }
                    case Target_UI.UIButton.EXIT_PLAY:
                        {
                            OnExitPlay();
                            break;
                        }
                    case Target_UI.UIButton.EXIT_ENDLESS:
                        {
                            OnExitEndless();
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
                    DestructionController.Instance.BlowUpObject( a_ContactScenario.Target.gameObject );
                    a_ContactScenario.Arrow.TriggerDespawn( 4.0f );
                }
                else
                {
                    DestructionController.Instance.BlowUpObject( a_ContactScenario.Arrow.gameObject );
                }
            }
        }

        // No target hit of any kind.
        else
        {
            a_ContactScenario.Arrow.TriggerDespawn( 4.0f );
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

    //-------------------------------Game Flags---------------------------------------------------
    #pragma warning disable 0414

    private bool m_BowPickedUp;

    #pragma warning restore 0414
}