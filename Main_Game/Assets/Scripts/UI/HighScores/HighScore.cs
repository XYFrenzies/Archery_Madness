using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class HighScore : Singleton<HighScore>
{
    [Tooltip("Max amount  to have in the highscores.")]
    [SerializeField] private int maxAmountInScores = 5;
    [SerializeField] private Text m_text = null;
    [Tooltip("Text displayed before the array of highscores")]
    [SerializeField] private string beforeDisplayHighScores = "";
    [Tooltip("Text displayed after the array of highscores")]
    [SerializeField] private string afterDisplayHighScores = "";
    private HighScore_Info[] m_highScores;
    // Start is called before the first frame update
    void Start()
    {
        m_highScores = new HighScore_Info[maxAmountInScores];
        for (int i = 0; i < m_highScores.Length; i++)
        {
            m_highScores[i] = new HighScore_Info();
            m_highScores[i].positionOnLadder = i + 1;
            m_highScores[i].score = -1;
        }
    }
    private void Update()
    {
        if(m_text != null)
            DisplayHighScores();
    }
    //If the max amount within the score system has been met.
    private bool MoreThanValueInArray(int score)
    {
        for (int i = 0; i < m_highScores.Length; i++)
        {
            if (score > m_highScores[i].score)
            {
                m_highScores[i].score = -1;
                m_highScores[i].positionOnLadder = 0;
                return true;
            }
        }
        return false;
    }
    //This sorts the array once a new entry has been added.
    private void SortArray()
    {
        foreach (var item in m_highScores)
        {
            item.positionOnLadder = 0;
        }
        Array.Sort(m_highScores);
        for (int i = 0; i < m_highScores.Length; i++)
        {
            if (m_highScores[i] != null)
                m_highScores[i].positionOnLadder = i + 1;
        }
    }
    //Adds a new score to the highscores.
    public void AddToHighScore(int score)
    {
        if (m_highScores.Length >= maxAmountInScores)
        {
            if (!MoreThanValueInArray(score))
                return;
        }
        HighScore_Info newHS = new HighScore_Info();
        newHS.score = score;
        for (int i = 0; i < m_highScores.Length; i++)
        {
            if (m_highScores[i].score == -1)
            {
                m_highScores[i] = newHS;
                break;
            }
        }
        SortArray();
    }
    //Gets the highestscore.
    public int GetHighestScore() 
    {
        return m_highScores[0].score;
    }
    //Gets a highscore.
    public int GetAHighScore(int i)
    {
        return m_highScores[i].score;
    }
    //Displays the highscores that are within the array.
    public void DisplayHighScores() 
    {
        m_text.text = beforeDisplayHighScores + "\n";
        foreach (var item in m_highScores)
        {
            m_text.text += item.positionOnLadder + "   " + item.score + "\n";
        }
        m_text.text = m_text.text + afterDisplayHighScores;
           
    }
}
