using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_GlassBird : Target
{
    public ObjectPool< Target_GlassBird > Pool;

    private void Awake()
    {
        Type = TargetType.GLASS;
        GetComponent< ShatterObject >()?.SetOnDisable( OnShatterDisable );
    }

    private void OnShatterDisable( GameObject gameObject )
    {
        Destroy( gameObject );
    }
}
