using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController
{
    public int Minutes { get; private set; } = 0;
    public int Seconds { get; private set; } = 0;

    public bool IsActive { get { return m_TimerCoroutine != null; } }

    public TimeController()
    {
        m_Wait = new WaitForSeconds( 1.0f );
    }

    public void StartTimer()
    {
        m_TimerCoroutine = GameStateManager.Instance.StartCoroutine( Begin() );
    }

    public void StopTimer()
    {
        GameStateManager.Instance.StopCoroutine( m_TimerCoroutine );
        m_TimerCoroutine = null;
    }

    public void PauseTimer()
    {
        m_Paused = true;
    }

    public void ResumeTimer()
    {
        m_Paused = false;
    }

    public void ToggleTimer()
    {
        m_Paused = !m_Paused;
    }

    private IEnumerator Begin()
    {
        while ( m_Paused )
        {
            yield return m_Wait;
        }
        
        yield return m_Wait;

        if ( ++Seconds > 59 )
        {
            Seconds = 0;
            ++Minutes;
        }
    }

    private bool m_Paused;
    private WaitForSeconds m_Wait;
    private Coroutine m_TimerCoroutine;
}
