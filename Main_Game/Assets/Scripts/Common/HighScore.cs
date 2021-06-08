using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ Serializable ]
public class HighScore : IComparable
{
    public int Value { get; private set; }

    public void Increment()
    {
        ++Value;
    }

    public void Increment( int a_Count )
    {
        if ( a_Count < 1 )
        {
            return;
        }

        Value += a_Count;
    }

    public void Decrement()
    {
        if ( Value == 0 )
        {
            return;
        }

        --Value;
    }

    public void Decrement( int a_Count )
    {
        if ( a_Count < 1 )
        {
            return;
        }

        Value -= a_Count;
        Value = Math.Max( 0, Value );
    }

    public int CompareTo( object a_Object )
    {
        return -( a_Object as HighScore ).Value.CompareTo( Value );
    }
}
