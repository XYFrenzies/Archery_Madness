using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkerDialogueController : Singleton< BarkerDialogueController >
{
    public Animator Animator;

    public enum BarkerDialogue
    {
        TakingTooLong,
        TimesUp,
        FriendlyFire,
        PlayerMiss,
        PlayerHit,
        LowScore,
        HighScore
    }

    public void TriggerBarker( BarkerDialogue a_Dialogue )
    {
        Animator.SetTrigger( a_Dialogue.ToString() );
        SoundPlayer.Instance.Play( a_Dialogue.ToString(), "Barker", 1.0f );
    }
}
