using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Rigidbody objToShoot = null;
    [SerializeField] private float moveSpeed = 10;

    private void Start()
    {
        objToShoot = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        objToShoot.AddForce(Camera.main.transform.forward * moveSpeed);
    }
    private void OnCollisionEnter()
    {
        objToShoot.velocity = Vector3.zero;
        objToShoot.gameObject.SetActive(false);
    }
}
