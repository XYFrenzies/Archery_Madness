using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ ExecuteInEditMode ]
[ RequireComponent( typeof ( LineRenderer ) ) ]
public class StringRenderer : MonoBehaviour
{
    [ Header( "Settings" ) ]
    public Gradient PullColour = null;

    [ Header( "References" ) ]
    public PullMeasurer PullMeasurer = null;

    [ Header( "Render Positions" ) ]
    public Transform Start = null;
    public Transform Middle = null;
    public Transform End = null;

    private LineRenderer LineRenderer = null;

    private void Awake()
    {
        LineRenderer = GetComponent< LineRenderer >();
    }

    private void Update()
    {
        if ( Application.isEditor && !Application.isPlaying )
        {
            UpdatePositions();
        }
    }

    private void OnEnable()
    {
        Application.onBeforeRender += UpdatePositions;
        PullMeasurer.Pulled.AddListener( UpdateColour );
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= UpdatePositions;
        PullMeasurer.Pulled.RemoveListener( UpdateColour );
    }

    private void UpdatePositions()
    {
        Vector3[] positions = new Vector3[] { Start.position, Middle.position, End.position };
        LineRenderer.SetPositions( positions );
    }

    private void UpdateColour( Vector3 a_PullPosition, float a_PullAmount )
    {
        Color color = PullColour.Evaluate( a_PullAmount );
        LineRenderer.material.color = color;
    }
}
