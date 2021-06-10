using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_PostPlay : GameState
{
    public GameState_PostPlay()
    {
        ThisState = State.POST_PLAY;
    }

    protected override void OnBegin()
    {

    }

    protected override IEnumerator OnProcess()
    {
        yield return null;
    }

    protected override void OnEnd()
    {

    }
}
