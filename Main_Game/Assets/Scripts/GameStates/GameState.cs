using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    public enum State
    {
        NONE,
        MENU,
        PRE_TUTORIAL,
        TUTORIAL,
        POST_TUTORIAL,
        PRE_PLAY,
        PLAY,
        POST_PLAY,
        PRE_ENDLESS,
        ENDLESS,
        POST_ENDLESS,
        SCORE
    }

    public virtual State ThisState { get; protected set; } = State.MENU;

    public void Start()
    {
        m_OnInitiate = GameStateManager.Instance.StartCoroutine( OnStart() );
        GameStateManager.Instance.Current = ThisState;
    }

    public void ForceStop()
    {
        if ( m_OnInitiate != null )
        {
            GameStateManager.Instance.StopCoroutine( m_OnInitiate );
            GameStateManager.Instance.Current = State.NONE;
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
