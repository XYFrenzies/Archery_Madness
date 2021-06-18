using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Barker : Target
{
    private void Awake()
    {
        Type = TargetType.BARKER;
    }

    public override void OnArrowHit( Arrow a_Arrow )
    {
        BarkerDialogueController.Instance.TriggerBarker( BarkerDialogueController.BarkerDialogue.FriendlyFire );
    }
}
