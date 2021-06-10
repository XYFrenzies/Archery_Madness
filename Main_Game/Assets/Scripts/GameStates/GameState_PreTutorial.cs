using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_PreTutorial : GameState
{
    public GameState_PreTutorial()
    {
        ThisState = State.PRE_TUTORIAL;
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
