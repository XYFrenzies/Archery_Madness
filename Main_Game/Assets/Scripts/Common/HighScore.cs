using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ Serializable ]
public class HighScore : IComparable
{
    public int m_Score;

    public void Increment()
    {
        ++m_Score;
    }

    public void Decrement()
    {
        if ( m_Score == 0 )
        {
            return;
        }

        --m_Score;
    }

    public int CompareTo( object a_Object )
    {
        return -( a_Object as HighScore ).m_Score.CompareTo( m_Score );
    }
}
