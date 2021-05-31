using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnShot : MonoBehaviour
{
    [SerializeField] private GameObject shotPosition = null;
    [SerializeField] private GameObject objToShoot = null;
    // Update is called once per frame
    void Update()
    {
        if (objToShoot != null && Input.GetMouseButton(0))
        {
            objToShoot.transform.position = shotPosition.transform.position;
            objToShoot.transform.rotation = shotPosition.transform.rotation;
            objToShoot.SetActive(true);
        }
    }
}
