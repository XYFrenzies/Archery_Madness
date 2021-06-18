using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_WoodBird : Target
{
    public ObjectPool< Target_WoodBird > Pool;

    private void Awake()
    {
        Type = TargetType.WOOD;
        GetComponent< ShatterObject >()?.SetOnDisable( OnShatterDisable );
    }

    private void OnShatterDisable( GameObject gameObject )
    {
        Destroy( gameObject );
    }
}
