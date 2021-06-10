using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Play : GameState
{
    public GameState_Play()
    {
        ThisState = State.PLAY;
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
