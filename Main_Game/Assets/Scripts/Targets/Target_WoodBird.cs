using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_WoodBird : Target
{
    private void Awake()
    {
        m_TargetType = TargetType.WOOD;
    }

    public override void OnArrowContact(Arrow a_ContactingArrow)
    {

    }
}
