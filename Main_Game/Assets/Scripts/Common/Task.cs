using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public bool IsComplete { get; private set; } = false;
    public bool IsChainComplete { get; private set; } = false;

    public Task OnComplete
    {
        get
        {
            return m_OnComplete;
        }
        set
        {
            m_OnComplete = value;

            if ( m_OnComplete != null )
            {
                m_OnComplete.m_Caller = this;
            }
        }
    }

    public Task( Func< IEnumerator > a_Task )
    {
        m_Task = a_Task;
    }

    public Task( Func< IEnumerator > a_Task, Task a_OnComplete )
    {
        m_Task = a_Task;
        OnComplete = a_OnComplete;
    }

    public void Trigger()
    {
        GameStateManager.Instance.StartCoroutine( OnTrigger() );
    }

    private IEnumerator OnTrigger()
    {
        yield return GameStateManager.Instance.StartCoroutine( m_Task.Invoke() );
        IsComplete = true;

        if ( m_OnComplete != null )
        {
            m_OnComplete.Trigger();
        }
        else
        {
            IsChainComplete = true;
            Task task = m_Caller;
            
            while ( task != null )
            {
                task.IsChainComplete = true;
                task = task.m_Caller;
            }
        }
    }

    private Func< IEnumerator > m_Task;
    private Task m_Caller;
    private Task m_OnComplete;
}