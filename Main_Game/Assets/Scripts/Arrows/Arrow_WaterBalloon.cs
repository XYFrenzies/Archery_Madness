using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_WaterBalloon : Arrow
{
    private new void Awake()
    {
        base.Awake();
        m_ArrowType = ArrowType.WATER;
    }
}
