using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_PreEndless : GameState
{
    public GameState_PreEndless()
    {
        ThisState = State.PRE_ENDLESS;
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
