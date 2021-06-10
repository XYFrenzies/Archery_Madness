using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public bool ResetOnChainComplete;
    public bool TriggerRepeat;

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

    public void Reset()
    {
        if ( !IsChainComplete )
        {
            return;
        }

        IsChainComplete = false;
        IsComplete = false;
        Task task = m_OnComplete;

        while ( task != null )
        {
            task.IsChainComplete = false;
            task.IsComplete = false;
            task = task.m_OnComplete;
        }
    }

    public static Task operator +( Task a_LHS, Task a_RHS )
    {
        if ( a_LHS == null || a_RHS == null )
        {
            return a_LHS;
        }

        Task lastTask = a_LHS;

        while ( a_LHS.m_OnComplete != null )
        {
            lastTask = lastTask.m_OnComplete;
        }

        lastTask.m_OnComplete = a_RHS;
        return a_LHS;
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
            Task thisTask = this;
            
            while ( task != null )
            {
                task.IsChainComplete = true;
                thisTask = task;
                task = task.m_Caller;
            }

            if ( ResetOnChainComplete )
            {
                thisTask.Reset();

                if ( TriggerRepeat )
                {
                    thisTask.Trigger();
                }
            }
        }
    }

    private Func< IEnumerator > m_Task;
    private Task m_Caller;
    private Task m_OnComplete;
}