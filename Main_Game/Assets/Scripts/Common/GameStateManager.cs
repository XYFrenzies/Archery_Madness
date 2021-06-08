using System.Collections.Generic;
using System.Collections;
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

    public ScoreController ScoreController { get; private set; }

    private void Awake()
    {
        m_States = new Dictionary< GameState.State, GameState >();
        m_States.Add( GameState.State.MENU, new GameState_Menu() );
        m_States.Add( GameState.State.PRE_GAME, new GameState_PreGame() );
        m_States.Add( GameState.State.IN_GAME, new GameState_InGame() );
        m_States.Add( GameState.State.POST_GAME, new GameState_PostGame() );
        m_States.Add( GameState.State.SCORE, new GameState_Score() );
        m_CurrentState = m_States[ GameState.State.MENU ];

        TimeController = new TimeController();
        ScoreController = GetComponent< ScoreController >();
    }

    private void Update()
    {
        m_CurrentState.OnProcess();
    }

    public GameState.State Current
    {
        get
        {
            return m_CurrentState.ThisState;
        }
    }

    //-----------------------Callbacks------------------------
    public void OnPlayGame()
    {
        // Called when "PlayGame" target is shot.
    }

    public void OnPlayTutorial()
    {
        // Called when "PlayTutorial" target is shot.
    }

    public void OnExitTutorial()
    {
        // Called when "Exit Tutorial" target is shot.
    }

    public void OnExit()
    {
        // Called when "Exit" target is shot.
    }

    // Called when Bow is picked up.
    public void OnBowPickup( BaseInteractionEventArgs a_Args )
    {
        m_BowPickedUp = true;
    }

    // Called when Bow is dropped.
    public void OnBowDrop( BaseInteractionEventArgs a_Args )
    {
        m_BowPickedUp = false;
    }

    public void RegisterShot( ContactScenario a_ContactScenario )
    {
        // Called when arrow hits something.
    }

    //-------------------------------------------------------------

    // Fires end of current state, beginning of next state, and sets next state as current state.
    public void SwitchToState( GameState.State a_NextState )
    {
        m_CurrentState.OnEnd();
        m_CurrentState = m_States[ a_NextState ];
        m_CurrentState.OnBegin();
    }

    // Get State associated with the flag.
    public GameState GetState( GameState.State a_State )
    {
        return m_States[ a_State ];
    }

    private GameState m_CurrentState = null;
    private Dictionary< GameState.State, GameState > m_States;

    //-------------------------------Game Flags---------------------------------------------------
    private bool m_BowPickedUp;

}

public abstract class GameState
{
    public enum State
    {
        MENU,
        PRE_GAME,
        IN_GAME,
        POST_GAME,
        SCORE
    }

    public virtual State ThisState { get; protected set; } = State.MENU;

    public virtual void OnBegin()
    {

    }

    public virtual void OnProcess()
    {

    }

    public virtual void OnEnd()
    {

    }
}

public sealed class GameState_Menu : GameState
{
    public GameState_Menu()
    {
        ThisState = State.MENU;
    }

    public override void OnBegin()
    {
        
    }

    public override void OnProcess()
    {
        
    }

    public override void OnEnd()
    {

    }
}

public sealed class GameState_PreGame : GameState
{
    public GameState_PreGame()
    {
        ThisState = State.PRE_GAME;
    }

    public override void OnBegin()
    {

    }

    public override void OnProcess()
    {

    }

    public override void OnEnd()
    {

    }
}

public sealed class GameState_InGame : GameState
{
    public GameState_InGame()
    {
        ThisState = State.IN_GAME;
    }

    public override void OnBegin()
    {

    }

    public override void OnProcess()
    {

    }

    public override void OnEnd()
    {

    }
}

public sealed class GameState_PostGame : GameState
{
    public GameState_PostGame()
    {
        ThisState = State.POST_GAME;
    }

    public override void OnBegin()
    {

    }

    public override void OnProcess()
    {

    }

    public override void OnEnd()
    {

    }
}

public sealed class GameState_Score : GameState
{
    public GameState_Score()
    {
        ThisState = State.SCORE;
    }

    public override void OnBegin()
    {

    }

    public override void OnProcess()
    {

    }

    public override void OnEnd()
    {

    }
}