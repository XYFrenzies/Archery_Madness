using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Score : GameState
{
    public GameState_Score()
    {
        ThisState = State.SCORE;
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
