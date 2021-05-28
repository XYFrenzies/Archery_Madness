using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public KeyCode OnInteractKey;

    public enum Side
    {
        LEFT,
        RIGHT
    }

    private void OnCollisionEnter(Collision a_Collision)
    {
        if (!a_Collision.gameObject.TryGetComponent(out ArrowSpawner spawner))
        {
            return;
        }

        spawner.OnPreInteract(this);
    }

    private void OnCollisionExit(Collision a_Collision)
    {
        if (!a_Collision.gameObject.TryGetComponent(out ArrowSpawner spawner))
        {
            return;
        }

        spawner.OnPostInteract(this);
    }

    private void OnCollisionStay(Collision a_Collision)
    {
        if (!a_Collision.gameObject.TryGetComponent(out ArrowSpawner spawner))
        {
            return;
        }

        if (Input.GetKeyDown(OnInteractKey))
        {
            spawner.OnInteract(this);
        }
    }

    public Side HandSide
    {
        get
        {
            return m_HandSide;
        }
    }

    private Side m_HandSide;
}
