using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Endless : GameState
{
    public GameState_Endless()
    {
        ThisState = State.ENDLESS;
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
