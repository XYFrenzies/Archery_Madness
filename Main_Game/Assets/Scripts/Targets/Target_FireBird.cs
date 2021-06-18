using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_FireBird : Target
{
    public ObjectPool< Target_FireBird > Pool;

    private void Awake()
    {
        Type = TargetType.FIRE;
        GetComponent< ShatterObject >()?.SetOnDisable( OnShatterDisable );
    }

    private void OnShatterDisable( GameObject gameObject )
    {
        Destroy( gameObject );
    }
}
