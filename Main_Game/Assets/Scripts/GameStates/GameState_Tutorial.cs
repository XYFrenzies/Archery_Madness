using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Tutorial : GameState
{
    public GameState_Tutorial()
    {
        ThisState = State.TUTORIAL;
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
