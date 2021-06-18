using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_UI : Target
{
    public ObjectPool< Target_UI > Pool;

    public enum UIButton
    {
        PLAY,
        EXIT
    }

    public UIButton ButtonType;

    private void Awake()
    {
        Type = TargetType.UI;
        GetComponent< ShatterObject >()?.SetOnDisable( OnShatterDisable );
    }

    private void OnShatterDisable( GameObject gameObject )
    {
        Destroy( gameObject );
    }
}
