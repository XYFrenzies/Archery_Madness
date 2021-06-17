using UnityEngine;
using static System.Linq.Enumerable;

public class Digit : MonoBehaviour 
{
    private class Shape
    {
        public Shape( params byte[] a_SegmentIndexes ) 
        {
            m_ShapeBits = ( byte )a_SegmentIndexes.Aggregate( 0, ( segmentBits, segmentIndex ) => 
                segmentBits | ( 1 << segmentIndex ) );
        }

        public bool HasSegment( int a_SegmentIndex ) 
        {
            int bit = 1 << a_SegmentIndex;
            return ( m_ShapeBits & bit ) == bit;
        }

        
        private readonly byte m_ShapeBits;
    }

    public bool IsOn { get; private set; }

    private void Awake() 
    {
        m_Renderers = gameObject.GetComponentsInChildren< Renderer >();
    }

    public void SetDisplay( DigitalDisplay a_Display )
    {
        m_Display = a_Display;
    }

    public void SetDigit( int a_Digit ) 
    {
        m_DigitShowing = a_Digit;
        TurnOn();
    }

    public void TurnOn() 
    {
        Shape shape = a_DigitShapes[ m_DigitShowing ];

        for ( int i = 0; i < m_SegmentCount; ++i ) 
        {
            Renderer segmentRenderer = m_Renderers[ i ];
            bool on = shape.HasSegment( i );
            segmentRenderer.material = on ? m_Display.DigitOnMaterial : 
                                            m_Display.DigitOffMaterial;
        }

        IsOn = true;
    }

    public void TurnOff() 
    {
        foreach ( Renderer renderer in m_Renderers )
        {
            renderer.material = m_Display.DigitOffMaterial;
        }

        IsOn = false;
    }
    
    private int m_DigitShowing;
    private const int m_SegmentCount = 7;
    private DigitalDisplay m_Display;
    [ SerializeField ] private Renderer[] m_Renderers;
    private static readonly Shape[] a_DigitShapes = 
    {
        new Shape(0,    2, 3, 4, 5, 6),
        new Shape(            4,    6),
        new Shape(0, 1, 2,    4, 5   ),    //   |    2    | 
        new Shape(0, 1, 2,    4,    6),    //   |3  ---  4| 
        new Shape(   1,    3, 4,    6),    //   |    1    | 
        new Shape(0, 1, 2, 3,       6),    //   |5  ---  6| 
        new Shape(0, 1, 2, 3,    5, 6),    //   |    0    | 
        new Shape(      2,    4,    6),    //       ---     
        new Shape(0, 1, 2, 3, 4, 5, 6),
        new Shape(0, 1, 2, 3, 4,    6)
    };
}
