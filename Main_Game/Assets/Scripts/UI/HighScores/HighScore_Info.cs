using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HighScore_Info: IComparable
{
    public int positionOnLadder;
    public int score;

    public HighScore_Info() 
    {
        positionOnLadder = 0;
        score = 0;
    }

    public int CompareTo(object obj)
    {
        HighScore_Info info = obj as HighScore_Info;
        return info.score.CompareTo(score);
    }
}
