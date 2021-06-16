using UnityEngine;
using System.Collections.Generic;
using System;

[ CreateAssetMenu( fileName = "NewAudioClipPool", menuName = "AssetPools/Audio Clip" ) ] 
public class AudioClipPool : ScriptableObject
{
    public int Count
    {
        get
        {
            return m_Clips.Length;
        }
    }

    public AudioClip Random
    {
        get
        {
            return m_Clips[ UnityEngine.Random.Range( 0, m_Clips.Length ) ];
        }
    }

    private void OnEnable()
    {
        if ( m_Clips == null )
        {
            m_Clips = new AudioClip[ 0 ];
        }
    }

    public AudioClip GetAt( int a_Index )
    {
        if ( a_Index < 0 || a_Index >= m_Clips.Length )
        {
            return default;
        }

        return m_Clips[ a_Index ];
    }

    [ SerializeField ] private AudioClip[] m_Clips;
}