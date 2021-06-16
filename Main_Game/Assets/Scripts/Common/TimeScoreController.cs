using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScoreController : Singleton< TimeScoreController >
{
    [ Range( 0, 10 ) ] public int TrackedScores;
    public DigitalDisplay DigitalDisplay;

    public HighScore CurrentScore
    {
        get
        {
            if ( m_CurrentScore == null )
            {
                ResetCurrentScore();
            }

            return m_CurrentScore;
        }
    }

    private void Awake()
    {
        ScoreObject[] scoreObjects = Resources.FindObjectsOfTypeAll< ScoreObject >();
        
        if ( scoreObjects.Length > 0 )
        {
            m_ScoreObject = scoreObjects[ 0 ];
        }
    }

    private void Start()
    {
        DigitalDisplay.TurnOffAll();
    }

    public void ResetCurrentScore()
    {
        m_CurrentScore = new HighScore();
    }

    public void SaveCurrentScore()
    {
        if ( m_CurrentScore == null )
        {
            return;
        }

        Insert( m_CurrentScore );
    }

    public void AddToScore( int a_Value )
    {
        CurrentScore.Add( a_Value );
        DigitalDisplay.SetNumber( 0, CurrentScore.Value );
    }

    public void TriggerTurnOnDisplay( int a_Score, int a_Minutes, int a_Seconds )
    {
        StartCoroutine( TurnOnDisplay( a_Score, a_Minutes, a_Seconds ) );
    }

    public IEnumerator TurnOnDisplay( int a_Score, int a_Minutes, int a_Seconds )
    {
        DigitalDisplay.SetNumber( 0, a_Score );
        DigitalDisplay.SetNumber( 1, a_Minutes );
        DigitalDisplay.SetNumber( 2, a_Seconds );

        for ( int i = 0; i < 2; ++i )
        {
            DigitalDisplay.TurnOnAll();
            yield return new WaitForSeconds( 0.5f );
            DigitalDisplay.TurnOffAll();
            yield return new WaitForSeconds( 0.5f );
        }

        DigitalDisplay.TurnOnAll();
    }

    public HighScore GetHighScore( int a_Index )
    {
        if ( a_Index < 0 || a_Index >= m_ScoreObject.HighScores.Length )
        {
            return null;
        }

        return m_ScoreObject.HighScores[ a_Index ];
    }

    public HighScore[] GetHighScores()
    {
        return m_ScoreObject.HighScores;
    }

    public void Remove( HighScore a_HighScore )
    {
        int foundIndex = Array.FindIndex( m_ScoreObject.HighScores, score => score == a_HighScore );
        
        if ( foundIndex == -1 )
        {
            return;
        }

        m_ScoreObject.HighScores[ foundIndex ] = 
            m_ScoreObject.HighScores[ m_ScoreObject.HighScores.Length - 1 ];
        Array.Resize( ref m_ScoreObject.HighScores, m_ScoreObject.HighScores.Length - 1 );
    }

    public void RemoveAt( int a_Index )
    {
        if ( a_Index < 0 || a_Index >= m_ScoreObject.HighScores.Length )
        {
            return;
        }

        m_ScoreObject.HighScores[ a_Index ] = 
            m_ScoreObject.HighScores[ m_ScoreObject.HighScores.Length - 1 ];
        Array.Resize( ref m_ScoreObject.HighScores, m_ScoreObject.HighScores.Length - 1 );
    }

    private void Insert( HighScore a_HighScore )
    {
        Array.Resize( ref m_ScoreObject.HighScores, m_ScoreObject.HighScores.Length + 1 );
        m_ScoreObject.HighScores[ m_ScoreObject.HighScores.Length - 1 ] = a_HighScore;
        Sort();

        if ( m_ScoreObject.HighScores.Length > TrackedScores )
        {
            Array.Resize( ref m_ScoreObject.HighScores, m_ScoreObject.HighScores.Length - 1 );
        }
    }

    private void Sort()
    {
        Array.Sort( m_ScoreObject.HighScores );
    }

    private ScoreObject m_ScoreObject;
    private HighScore m_CurrentScore;
}
