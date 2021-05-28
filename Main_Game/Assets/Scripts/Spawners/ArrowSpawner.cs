﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public Arrow.ArrowType ArrowType;
    [Range(1, 100)] public int ArrowCapacity;

    private void Awake()
    {
        //Need to integrate the poolsize of each arrow type.
        switch (ArrowType)
        {
            case Arrow.ArrowType.BROAD:
                {
                    m_ArrowPool = new ObjectPool<Arrow>(GenerateNewArrow<Arrow_Broadhead>, arrow => arrow.OnActivate(), arrow => arrow.OnDeactivate());
                    break;
                }
            case Arrow.ArrowType.HAMMER:
                {
                    m_ArrowPool = new ObjectPool<Arrow>(GenerateNewArrow<Arrow_Hammerhead>, arrow => arrow.OnActivate(), arrow => arrow.OnDeactivate());
                    break;
                }
            case Arrow.ArrowType.WATER:
                {
                    m_ArrowPool = new ObjectPool<Arrow>(GenerateNewArrow<Arrow_WaterBalloon>, arrow => arrow.OnActivate(), arrow => arrow.OnDeactivate());
                    break;
                }
        }
    }

    private void OnCollisionEnter(Collision a_Collision)
    {
        if (!a_Collision.gameObject.TryGetComponent(out HandController controller))
        {
            return;
        }

        switch (controller.HandSide)
        {
            case HandController.Side.LEFT:
                {
                    if (!m_LeftHand)
                    {
                        m_LeftHand = true;
                        OnPreInteract(GameStateManager.Instance.LeftHand);
                    }

                    break;
                }
            case HandController.Side.RIGHT:
                {
                    if (!m_RightHand)
                    {
                        m_RightHand = true;
                        OnPreInteract(GameStateManager.Instance.RightHand);
                    }

                    break;
                }
        }
    }

    private void OnCollisionExit(Collision a_Collision)
    {
        if (!a_Collision.gameObject.TryGetComponent(out HandController controller))
        {
            return;
        }

        switch (controller.HandSide)
        {
            case HandController.Side.LEFT:
                {
                    if (m_LeftHand)
                    {
                        m_LeftHand = false;
                        OnPostInteract(GameStateManager.Instance.LeftHand);
                    }

                    break;
                }
            case HandController.Side.RIGHT:
                {
                    if (m_RightHand)
                    {
                        m_RightHand = false;
                        OnPostInteract(GameStateManager.Instance.RightHand);
                    }

                    break;
                }
        }
    }

    public void OnPreInteract(HandController a_Controller)
    {
        // Called when spawner CAN be interacted with.
    }

    public void OnInteract(HandController a_Controller)
    {
        // Called when spawner is interacted with.
    }

    public void OnPostInteract(HandController a_Controller)
    {
        // Called when spawner can NO LONGER be interacted with.
    }

    public T GenerateNewArrow<T>() where T : Arrow
    {
        return default;
    }

    private ObjectPool<Arrow> m_ArrowPool;
    private bool m_LeftHand;  //Current left hand inside of interaction zone.
    private bool m_RightHand;// Current right hand inside of interaction zone.
}