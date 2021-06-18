using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Pool : MonoBehaviour
{
    public GameObject PrefabbedPool;
    public Transform Position;
    public ObjectPool< Test_Object > Pool;
    public Test_Object CurrentlySelected;

    private void Awake()
    {
        Pool = new ObjectPool<Test_Object>( Create, OnActive, OnInactive );
        Pool.Populate( 2 );
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ( CurrentlySelected != null )
            {
                return;
            }

            if ( Pool.TrySpawn( out Test_Object test_Object ) )
            {
                CurrentlySelected = test_Object;
            }
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            if (Pool.Despawn(CurrentlySelected))
            {
                CurrentlySelected = null;
            }
        }
        else if ( Input.GetKeyDown( KeyCode.B ) && CurrentlySelected != null )
        {
            CurrentlySelected.GetComponent< ShatterObject >()?.TriggerExplosion();
        }
    }

    private Test_Object Create()
    {
        GameObject newObject = Instantiate( PrefabbedPool, new Vector3( 0, -10, 0 ), Quaternion.identity );
        newObject.GetComponent< Rigidbody >().isKinematic = true;
        Test_Object testObject = newObject.GetComponent< Test_Object >();
        testObject.Pool = this;

        return testObject;
    }

    private void OnActive(Test_Object obj)
    {
        obj.gameObject.SetActive( true );
        obj.transform.position = Position.position;
        obj.transform.rotation = Position.rotation;
    }

    private void OnInactive( Test_Object obj )
    {
        obj.transform.position = new Vector3( 0, -10, 0 );
        obj.transform.rotation = Quaternion.identity;
    }
}
