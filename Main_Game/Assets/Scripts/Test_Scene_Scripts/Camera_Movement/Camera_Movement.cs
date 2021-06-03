using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is a text class for movement outside of VR, it allows for the camera to move around the scene. 
/// This only works if the individual presses and holds the right click for the mouse.
/// </summary>
public class Camera_Movement : MonoBehaviour
{
    private Vector3 moveVector;
    [SerializeField] private float moveSpeed = 100.0f;
    private float rawAxisHorz = 0;
    private float rawAxisVert = 0;
    [SerializeField] private GameObject camObj = null;

    [SerializeField] private float speedHorizontalMouse = 2.0f;
    [SerializeField] private float speedVerticalMouse = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        moveVector = new Vector3(0, 0, 0);//Default
    }

    // Update is called once per frame
    void Update()
    {
        //If the mouse right click is held.
        if (Input.GetMouseButton(1))
        {
            //Sets the inputs to specific variables. With it being horizontal and vertical for WASD and arrow keys.
            rawAxisHorz = Input.GetAxisRaw("Horizontal");
            rawAxisVert = Input.GetAxisRaw("Vertical");
            //Stes the inputs to specific variables. With the mouse directions being applied to the speed.
            yaw += speedHorizontalMouse * Input.GetAxis("Mouse X");
            pitch -= speedVerticalMouse * Input.GetAxis("Mouse Y");
            //Setting the vector values to the inputs.
            moveVector.x = rawAxisHorz;
            moveVector.y = rawAxisVert;
            //Getting the direction in relation to the movement and angle of the camera.
            Vector3 input = camObj.transform.right * moveVector.x + camObj.transform.forward * moveVector.y;
            //If it isnt standing still.
            if (rawAxisHorz != 0 || rawAxisVert != 0)
                camObj.transform.position += moveSpeed * input * Time.deltaTime;
            //The direction in which the mouse is moving along the x and y axis.
            camObj.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }

}
