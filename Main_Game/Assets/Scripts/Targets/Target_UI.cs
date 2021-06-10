using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_UI : Target
{
    public enum UIButton
    {
        TUTORIAL,
        PLAY,
        ENDLESS,
        EXIT_TUTORIAL,
        EXIT_PLAY,
        EXIT_ENDLESS
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
