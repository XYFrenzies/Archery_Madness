using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_GlassBird : Target
{
    private void Awake()
    {
        m_TargetType = TargetType.GLASS;
    }

    public override void OnArrowContact(Arrow a_ContactingArrow)
    {
        
    }
}
