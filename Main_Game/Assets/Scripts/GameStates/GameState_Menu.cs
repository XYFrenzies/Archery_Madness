using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameState_Menu : GameState
{
    public GameState_Menu()
    {
        ThisState = State.MENU;
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
