using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_PostEndless : GameState
{
    public GameState_PostEndless()
    {
        ThisState = State.POST_ENDLESS;
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
