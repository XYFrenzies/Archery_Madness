using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton< MusicManager >
{
    public void PlayMenuMusic()
    {
        SoundPlayer.Instance.PlayRepeat( "MainMenuMusic", 0.3f, true );
    }

    public void PlayGamePlayMusic()
    {
        SoundPlayer.Instance.PlayRepeat( "GamePlayMusic", 0.3f, true );
    }
}