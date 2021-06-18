using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Object : MonoBehaviour, IResettable
{
    public Test_Pool Pool;

    private void Awake()
    {
        GetComponent< ShatterObject >()?.SetOnDisable( ResetExplosion );
    }

    public void ResetExplosion( GameObject gameObject )
    {
        gameObject.SetActive( false );
        Pool.Pool.Despawn( this );
        Pool.CurrentlySelected = null;
    }

    public void OnReset()
    {
        
    }
}
