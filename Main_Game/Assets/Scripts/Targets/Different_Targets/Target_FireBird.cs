using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_FireBird : Target
{
    private void Awake()
    {
        m_TargetType = TargetType.FIRE;
    }

    public override void OnArrowContact(Arrow a_ContactingArrow)
    {

    }
}
