using System;
using UnityEngine;
using System.Collections.Generic;

public class DigitalDisplay : MonoBehaviour
{
    public DigitGroup[] DigitGroups;
    public Material DigitOnMaterial;
    public Material DigitOffMaterial;

    private void Awake()
    {
        foreach ( DigitGroup group in DigitGroups )
        {
            foreach ( Digit digit in group.Digits )
            {
                digit.SetDisplay( this );
            }

            group.Setup( this );
        }
    }

    public void TurnOn( int a_Index )
    {
        if ( a_Index < 0 || a_Index >= DigitGroups.Length )
        {
            return;
        }

        DigitGroups[ a_Index ].TurnOnAll();
    }

    public void TurnOnAll()
    {
        foreach ( DigitGroup group in DigitGroups )
        {
            group.TurnOnAll();
        }
    }

    public void TurnOff( int a_Index )
    {
        if ( a_Index < 0 || a_Index >= DigitGroups.Length )
        {
            return;
        }

        DigitGroups[ a_Index ].TurnOffAll();
    }

    public void TurnOffAll()
    {
        foreach ( DigitGroup group in DigitGroups )
        {
            group.TurnOffAll();
        }
    }

    public void SetNumber( int a_GroupIndex, int a_Value )
    {
        if ( a_GroupIndex < 0 || a_GroupIndex >= DigitGroups.Length )
        {
            return;
        }

        DigitGroups[ a_GroupIndex ].SetValue( a_Value );
    }
}

[ Serializable ]
public class DigitGroup
{
    public Digit[] Digits;
    public Renderer[] Dividers;
    public int Number;

    public void Setup( DigitalDisplay a_Display )
    {
        for ( int i = 0; i < Digits.Length; ++i )
        {
            m_MaxValue += 9 * ( int )Mathf.Pow( 10, i );
        }

        m_Display = a_Display;
    }

    public void TurnOn( int a_Index )
    {
        if ( a_Index < 0 || a_Index >= Digits.Length )
        {
            return;
        }

        Digits[ a_Index ].TurnOn();
    }

    public void TurnOnAll()
    {
        foreach ( Digit digit in Digits )
        {
            digit.TurnOn();
        }

        foreach ( Renderer divider in Dividers )
        {
            divider.material = m_Display.DigitOnMaterial;
        }
    }

    public void TurnOff( int a_Index )
    {
        if ( a_Index < 0 || a_Index >= Digits.Length )
        {
            return;
        }

        Digits[ a_Index ].TurnOff();
    }

    public void TurnOffAll()
    {
        foreach ( Digit digit in Digits )
        {
            digit.TurnOff();
        }

        foreach ( Renderer divider in Dividers )
        {
            divider.material = m_Display.DigitOffMaterial;
        }
    }

    public void Increment()
    {
        SetValue( ++Number );
    }
    //Decrease amount to total.
    public void Decrement()
    {
        SetValue( --Number );
    }

    public void SetValue( int a_Value )
    {
        a_Value = Mathf.Min( m_MaxValue, a_Value );

        List< byte > digits = new List< byte >();

        if ( a_Value != 0 )
        {
            while( a_Value > 0 && digits.Count < Digits.Length )
            {
                byte val = ( byte )( a_Value % 10 );
                digits.Add( val );
                a_Value -= val;
                a_Value /= 10;

                if ( a_Value == 0 )
                {
                    break;
                }
            }
        }
        else
        {
            digits.Add( 0 );
        }
        

        if ( digits.Count < Digits.Length )
        {
            int count = digits.Count;

            for ( int i = 0; i < Digits.Length - count; ++i )
            {
                digits.Insert( digits.Count, 0 );
            }
        }

        for ( int i = 0; i < Digits.Length; ++i )
        {
            Digits[ i ].SetDigit( digits[ Digits.Length - i - 1 ] );
        }
    }

    private int m_MaxValue;
    private DigitalDisplay m_Display;
}