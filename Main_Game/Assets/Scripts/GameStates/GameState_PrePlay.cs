using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_PrePlay : GameState
{
    public GameState_PrePlay()
    {
        ThisState = State.PRE_PLAY;
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
