using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Broadhead : Arrow
{
    private void Awake()
    {
        m_ArrowType = ArrowType.BROAD;
    }
}
