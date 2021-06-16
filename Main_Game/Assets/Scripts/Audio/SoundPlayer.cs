using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : Singleton< SoundPlayer >
{
    public AudioSource[] AudioSources;
    public AudioClipPool[] AudioPools;

    private void Awake()
    {
        m_ClipPools = new Dictionary< string, AudioClipPool >();
        m_SoundSources = new Dictionary< string, AudioSource >();
        foreach ( AudioSource source in AudioSources )
        {
            m_SoundSources.Add( source.gameObject.name, source );
        }
        foreach ( AudioClipPool pool in AudioPools )
        {
            m_ClipPools.Add( pool.name, pool );
        }
    }

    public void Play( string a_AudioPool, float a_Volume )
    {
        if ( m_ClipPools.TryGetValue( a_AudioPool, out AudioClipPool pool ) && 
             m_SoundSources.TryGetValue( "Main Camera", out AudioSource audioSource ) )
        {
            audioSource.clip = pool.Random;
            audioSource.loop = false;
            audioSource.volume = a_Volume;
            audioSource.Play();
        }
    }

    public void Play( string a_AudioPool, string a_AudioSource, float a_Volume, bool a_Windup = false )
    {
        if ( m_ClipPools.TryGetValue( a_AudioPool, out AudioClipPool pool ) && 
             m_SoundSources.TryGetValue( a_AudioSource, out AudioSource audioSource ) )
        {
            audioSource.clip = pool.Random;
            audioSource.volume = a_Volume;
            audioSource.loop = false;

            if ( a_Windup )
            {
                StartCoroutine( WindSoundUp( audioSource, 1.0f ) );
            }

            audioSource.Play();
        }
    }

    public void PlayRepeat( string a_AudioPool, float a_Volume, bool a_Windup )
    {
        if ( m_ClipPools.TryGetValue( a_AudioPool, out AudioClipPool pool ) && 
             m_SoundSources.TryGetValue( "Main Camera", out AudioSource audioSource ) )
        {
            audioSource.clip = pool.Random;
            audioSource.loop = true;
            audioSource.volume = a_Volume;

            if ( a_Windup )
            {
                StartCoroutine( WindSoundUp( audioSource, 2.0f ) );
            }

            audioSource.Play();
        }
    }

    private IEnumerator WindSoundUp( AudioSource a_AudioSource, float a_Time )
    {
        a_AudioSource.pitch = 0.0f;

        while ( a_AudioSource.pitch < 1.0f )
        {
            yield return null;
            a_AudioSource.pitch += Time.deltaTime / a_Time;
        }

        a_AudioSource.pitch = 1.0f;

        while ( a_AudioSource.isPlaying )
        {
            yield return new WaitForSeconds( Random.Range( 4.0f, 20.0f ) );
            float time = 0.0f;

            while ( time < 1.0f )
            {
                yield return null;
                time += Time.deltaTime * 1.0f;
                a_AudioSource.pitch = Mathf.Sin( time * 2 * Mathf.PI ) * 0.3f + 1.0f;
            }
        }
    }

    public void PlayRepeat( string a_AudioPool, string a_AudioSource, float a_Volume )
    {
        if ( m_ClipPools.TryGetValue( a_AudioPool, out AudioClipPool pool ) && 
             m_SoundSources.TryGetValue( a_AudioSource, out AudioSource audioSource ) )
        {
            audioSource.clip = pool.Random;
            audioSource.loop = true;
            audioSource.volume = a_Volume;
            audioSource.Play();
        }
    }

    public void PlayAtLocation( string a_AudioPool, Vector3 a_Location, float a_Volume )
    {
        if ( m_ClipPools.TryGetValue( a_AudioPool, out AudioClipPool pool ) )
        {
            AudioSource.PlayClipAtPoint( pool.Random, a_Location, a_Volume );
        }
    }

    public AudioClip GetClip( string a_AudioPool )
    {
        if ( m_ClipPools.TryGetValue( a_AudioPool, out AudioClipPool pool ) )
        {
            return pool.Random;
        }

        return null;
    }

    public AudioSource GetSource( string a_AudioSource )
    {
        if ( m_SoundSources.TryGetValue( a_AudioSource, out AudioSource source ) )
        {
            return source;
        }

        return null;
    }
    
    private Dictionary< string, AudioClipPool > m_ClipPools;
    private Dictionary< string, AudioSource > m_SoundSources;
}

