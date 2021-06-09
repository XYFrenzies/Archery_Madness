using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_UI : Target
{
    public enum UIButton
    {
        PLAY_GAME,
        PLAY_TUTORIAL,
        EXIT_TO_MENU,
        EXIT_GAME
    }

    public UIButton ButtonType;

    private void Awake()
    {
        Type = TargetType.UI;
    }

    public override void OnArrowHit( Arrow a_Arrow )
    {
        base.OnArrowHit( a_Arrow );
    }
}
