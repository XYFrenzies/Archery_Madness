using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHittable : MonoBehaviour, IArrowHittable
{
    public float ForceAmount = 1.0f;
    public Material OtherMaterial = null;

    public void Hit( Arrow a_Arrow )
    {
        ApplyMaterial();
        ApplyForce( a_Arrow.transform.forward );
    }

    public void ApplyMaterial()
    {
        MeshRenderer meshRenderer = GetComponent< MeshRenderer >();
        meshRenderer.material = OtherMaterial;
    }

    private void ApplyForce( Vector3 a_Direciton )
    {
        Rigidbody rigidbody = GetComponent< Rigidbody >();
        rigidbody.AddForce( a_Direciton * ForceAmount );
    }
}
