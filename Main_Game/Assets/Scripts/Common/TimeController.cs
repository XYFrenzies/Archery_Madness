using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeController
{
    public int Minutes { get; private set; } = 0;
    public int Seconds { get; private set; } = 0;

    public bool IsActive { get { return m_IsActive; } }

    public float Progression
    {
        get
        {
            return 1.0f - ( float )( Minutes * 60 + Seconds ) / m_BeginningSeconds;
        }
    }

    public UnityEvent OnSecondElapsed = new UnityEvent();
    public UnityEvent OnMinuteElapsed = new UnityEvent();
    public UnityEvent OnTimeUp = new UnityEvent();

    public TimeController()
    {
        m_Wait = new WaitForSeconds( 1.0f );
    }

    public void StartTimer( int a_Minutes, int a_Seconds )
    {
        Minutes = a_Minutes;
        Seconds = a_Seconds;
        m_BeginningSeconds = a_Minutes * 60 + a_Seconds;
        m_IsActive = true;
        GameStateManager.Instance.StartCoroutine( Begin() );
    }

    public void StopTimer()
    {
        m_IsActive = false;
        TimeScoreController.Instance.DigitalDisplay.SetNumber( 0, 0 );
        TimeScoreController.Instance.DigitalDisplay.SetNumber( 1, 0 );
        TimeScoreController.Instance.DigitalDisplay.SetNumber( 2, 0 );
        TimeScoreController.Instance.DigitalDisplay.TurnOffAll();
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
        while ( m_IsActive )
        {
            while ( m_Paused )
            {
                yield return m_Wait;
            }
            
            yield return m_Wait;
            OnSecondElapsed.Invoke();

            if ( --Seconds < 0 )
            {
                Seconds = 59;
                OnMinuteElapsed.Invoke();

                if ( --Minutes == 0 && Seconds == 0 )
                {
                    m_IsActive = false;
                    OnTimeUp.Invoke();
                }

                TimeScoreController.Instance.DigitalDisplay.SetNumber( 1, Minutes );
            }
            
            TimeScoreController.Instance.DigitalDisplay.SetNumber( 2, Seconds );
        }
    }

    private bool m_Paused;
    private bool m_IsActive;
    private int m_BeginningSeconds;
    private WaitForSeconds m_Wait;
}
