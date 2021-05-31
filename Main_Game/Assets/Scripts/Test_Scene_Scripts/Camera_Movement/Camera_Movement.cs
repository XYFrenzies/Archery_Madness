using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    private Vector3 moveVector;
    [SerializeField]private float moveSpeed = 100.0f;
    private float rawAxisHorz = 0;
    private float rawAxisVert = 0;
    [SerializeField] private GameObject camObj;

    [SerializeField] private float speedHorizontalMouse = 2.0f;
    [SerializeField] private float speedVerticalMouse = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        moveVector = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        rawAxisHorz = Input.GetAxisRaw("Horizontal");
        rawAxisVert = Input.GetAxisRaw("Vertical");
        yaw += speedHorizontalMouse * Input.GetAxis("Mouse X");
        pitch -= speedVerticalMouse * Input.GetAxis("Mouse Y");
        moveVector.x = rawAxisHorz;
        moveVector.y = rawAxisVert;
        Vector3 input = camObj.transform.right * moveVector.x + camObj.transform.forward * moveVector.y;
        if (rawAxisHorz != 0 || rawAxisVert != 0)
            camObj.transform.position += moveSpeed * input * Time.deltaTime;
        camObj.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
    
}
