using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public enum ArrowType
    {
        BROAD,
        HAMMER,
        WATER
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When arrow collides with an object.
    }

    public ArrowType Type
    {
        get
        {
            return m_ArrowType;
        }
    }

    protected ArrowType m_ArrowType;
}
