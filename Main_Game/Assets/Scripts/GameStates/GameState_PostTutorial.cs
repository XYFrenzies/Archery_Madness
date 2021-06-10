using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_PostTutorial : GameState
{
    public GameState_PostTutorial()
    {
        ThisState = State.POST_TUTORIAL;
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
