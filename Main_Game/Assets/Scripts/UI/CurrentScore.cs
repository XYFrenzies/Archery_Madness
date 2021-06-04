using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurrentScore : Singleton<CurrentScore>
{

    [Tooltip("Text displayed before score")]
    [SerializeField] private string textBeforeScore = "";//Text before the score
    [Tooltip("Text displayed after score")]
    [SerializeField] private string textAfterScore = "";//Text after the score
    private int currentScore; //Current score of the game.
    private Text m_textScore;
    private void Start()
    {
        currentScore = 0;
        m_textScore = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        if(m_textScore != null)
            SetCurrentScore();
    }
    //Sets the score in the scene.
    private void SetCurrentScore()
    {
        m_textScore.text = textBeforeScore + currentScore.ToString() + textAfterScore;
    }
    //Creates the current score and compare it to all the other high scores.
    public void SetNewHighScore() 
    {
        HighScore.Instance.AddToHighScore(currentScore);
        currentScore = 0;//Set back to 0
    }
    //Returns the score that the player is on.
    public int GetScore() 
    {
        return currentScore;
    }
    //Increments the score by a specific amount
    public void IncrementScore(int amount) 
    {
        currentScore += amount;
    }
    //Decreases Score by a specific amount
    public void DecreaseScore(int amount)
    {
        currentScore -= amount;
    }
}
