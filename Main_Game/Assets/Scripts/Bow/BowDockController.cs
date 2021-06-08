using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowDockController : MonoBehaviour
{
    public Transform BowDock;

    public void DockBow()
    {
        GameStateManager.Instance.Bow.transform.position = BowDock.position;
        GameStateManager.Instance.Bow.transform.rotation = BowDock.rotation;
    }
}
