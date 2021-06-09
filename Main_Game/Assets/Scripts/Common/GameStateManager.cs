using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.XR;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

    public ScoreController ScoreController { get; private set; }

    private void Awake()
    {
        m_States = new Dictionary< GameState.State, GameState >();
        m_States.Add( GameState.State.MENU, new GameState_Menu() );
        m_States.Add( GameState.State.PRE_GAME, new GameState_PreGame() );
        m_States.Add( GameState.State.IN_GAME, new GameState_InGame() );
        m_States.Add( GameState.State.POST_GAME, new GameState_PostGame() );
        m_States.Add( GameState.State.PRE_TUTORIAL, new GameState_PreTutorial() );
        m_States.Add( GameState.State.IN_TUTORIAL, new GameState_InTutorial() );
        m_States.Add( GameState.State.POST_TUTORIAL, new GameState_PostTutorial() );
        m_States.Add( GameState.State.SCORE, new GameState_Score() );
        m_CurrentState = m_States[ GameState.State.MENU ];

        TimeController = new TimeController();
        ScoreController = GetComponent< ScoreController >();

        DontDestroyOnLoad( this );
        DontDestroyOnLoad( ScoreController );

        GetState( GameState.State.MENU ).Start();
    }

    public GameState.State Current
    {
        get
        {
            return m_CurrentState.ThisState;
        }
    }

    //-----------------------Callbacks------------------------

    // Called when Play Game target is shot.
    public void OnPlayGame()
    {
        // Called when "PlayGame" target is shot.
        GetState( GameState.State.MENU ).ForceStop();
        GetState( GameState.State.PRE_GAME ).Start();
    }

    // Called when Play Tutorial target is shot.
    public void OnPlayTutorial()
    {
        // Called when "PlayTutorial" target is shot.
    }

    // Called when Exit To Menu target is shot.
    public void OnExitToMenu()
    {
        // Called when "Exit Tutorial" target is shot.
    }

    // Called when Exit Game target is shot.
    public void OnExitGame()
    {
        // Called when "Exit" target is shot.
        Application.Quit();
    }

    // Called when Bow is picked up.
    public void OnBowPickup( BaseInteractionEventArgs a_Args )
    {
        m_BowPickedUp = true;
        Bow.SetPhysics( false );

        if ( m_BowRedockTimer != null )
        {
            StopCoroutine( m_BowRedockTimer );
            m_BowRedockTimer = null;
        }
    }

    // Called when Bow is dropped.
    public void OnBowDrop( BaseInteractionEventArgs a_Args )
    {
        m_BowPickedUp = false;
        Bow.SetPhysics( true );
        
        m_BowRedockTimer =  StartCoroutine( BowRedockTimer() );
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
                switch ( ( a_ContactScenario.Target as Target_UI ).ButtonType )
                {
                    case Target_UI.UIButton.PLAY_GAME:
                        {
                            OnPlayGame();
                            break;
                        }
                    case Target_UI.UIButton.PLAY_TUTORIAL:
                        {
                            OnPlayTutorial();
                            break;
                        }
                    case Target_UI.UIButton.EXIT_TO_MENU:
                        {
                            OnExitToMenu();
                            break;
                        }
                    case Target_UI.UIButton.EXIT_GAME:
                        {
                            OnExitGame();
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
            }
        }

        // No target hit of any kind.
        else
        {

        }
    }

    //-------------------------------------------------------------

    private IEnumerator BowRedockTimer()
    {
        yield return new WaitForSeconds( 2.0f );

        Bow.SetPhysics( false );
        BowDockController.DockBow();
    }

    // Get State associated with the flag.
    public GameState GetState( GameState.State a_State )
    {
        return m_States[ a_State ];
    }

    private GameState m_CurrentState = null;
    private Dictionary< GameState.State, GameState > m_States;
    private Coroutine m_BowRedockTimer;

    //-------------------------------Game Flags---------------------------------------------------
    #pragma warning disable 0414

    private bool m_BowPickedUp;

    #pragma warning restore 0414
}

public abstract class GameState
{
    public enum State
    {
        MENU,
        PRE_GAME,
        IN_GAME,
        POST_GAME,
        PRE_TUTORIAL,
        IN_TUTORIAL,
        POST_TUTORIAL,
        SCORE
    }

    public virtual State ThisState { get; protected set; } = State.MENU;

    public void Start()
    {
        m_OnInitiate = GameStateManager.Instance.StartCoroutine( OnStart() );
    }

    public void ForceStop()
    {
        if ( m_OnInitiate != null )
        {
            GameStateManager.Instance.StopCoroutine( m_OnInitiate );
            OnEnd();
        }
    }

    private IEnumerator OnStart()
    {
        OnBegin();
        yield return OnProcess();
        OnEnd();
    }

    protected virtual void OnBegin()
    {

    }

    protected virtual IEnumerator OnProcess()
    {
        yield return null;
    }

    protected virtual void OnEnd()
    {

    }

    private Coroutine m_OnInitiate;
}

public sealed class GameState_Menu : GameState
{
    public GameState_Menu()
    {
        ThisState = State.MENU;
    }

    protected override void OnBegin()
    {

    }

    protected override IEnumerator OnProcess()
    {
        yield return null;
    }

    protected override void OnEnd()
    {

    }
}

public sealed class GameState_PreGame : GameState
{
    public GameState_PreGame()
    {
        ThisState = State.PRE_GAME;
    }

    protected override void OnBegin()
    {

    }

    protected override IEnumerator OnProcess()
    {
        yield return null;
    }

    protected override void OnEnd()
    {

    }
}

public sealed class GameState_InGame : GameState
{
    public GameState_InGame()
    {
        ThisState = State.IN_GAME;
    }

    protected override void OnBegin()
    {

    }

    protected override IEnumerator OnProcess()
    {
        yield return null;
    }

    protected override void OnEnd()
    {

    }
}

public sealed class GameState_PostGame : GameState
{
    public GameState_PostGame()
    {
        ThisState = State.POST_GAME;
    }

    protected override void OnBegin()
    {

    }

    protected override IEnumerator OnProcess()
    {
        yield return null;
    }

    protected override void OnEnd()
    {

    }
}

public sealed class GameState_PreTutorial : GameState
{
    public GameState_PreTutorial()
    {
        ThisState = State.PRE_TUTORIAL;
    }

    protected override void OnBegin()
    {

    }

    protected override IEnumerator OnProcess()
    {
        yield return null;
    }

    protected override void OnEnd()
    {

    }
}

public sealed class GameState_InTutorial : GameState
{
    public GameState_InTutorial()
    {
        ThisState = State.IN_TUTORIAL;
    }

    protected override void OnBegin()
    {

    }

    protected override IEnumerator OnProcess()
    {
        yield return null;
    }

    protected override void OnEnd()
    {

    }
}

public sealed class GameState_PostTutorial : GameState
{
    public GameState_PostTutorial()
    {
        ThisState = State.POST_TUTORIAL;
    }

    protected override void OnBegin()
    {

    }

    protected override IEnumerator OnProcess()
    {
        yield return null;
    }

    protected override void OnEnd()
    {

    }
}

public sealed class GameState_Score : GameState
{
    public GameState_Score()
    {
        ThisState = State.SCORE;
    }

    protected override void OnBegin()
    {

    }

    protected override IEnumerator OnProcess()
    {
        yield return null;
    }

    protected override void OnEnd()
    {

    }
}