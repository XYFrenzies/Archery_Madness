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

    public void Decrement()
    {
        if ( Value == 0 )
        {
            return;
        }

        --Value;
    }

    public void Add( int a_Count )
    {
        Value += a_Count;
        Value = Mathf.Max( Value, 0 );
    }

    public int CompareTo( object a_Object )
    {
        return -( a_Object as HighScore ).Value.CompareTo( Value );
    }
}
