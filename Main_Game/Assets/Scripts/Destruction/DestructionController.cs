using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionController : MonoBehaviour
{
    [ Range( 0.0f, 10000.0f ) ] public float ExplodeForce = 500.0f;
    [ Range( 1, 20 ) ] public int Cuts = 3;

    public void BlowUp()
    {
        transform.SetParent( null );
        transform.DetachChildren();

        foreach ( MeshDestroy meshDestroyer in m_MeshDestroyers )
        {
            if ( meshDestroyer.gameObject.TryGetComponent( out Rigidbody rigidbody ) )
            {
                rigidbody.isKinematic = false;
            }

            meshDestroyer.ExplodeForce = ExplodeForce;
            meshDestroyer.CutCascades = Cuts;
            meshDestroyer.DestroyMesh( m_DespawnTime );
        }

        Destroy( gameObject );
    }

    [ SerializeField ] private List< MeshDestroy > m_MeshDestroyers;
    private float m_DespawnTime = 1.0f;
}
