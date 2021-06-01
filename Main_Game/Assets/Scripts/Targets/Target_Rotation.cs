using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target_Rotation : MonoBehaviour
{
    [Tooltip("Changing the speed of the motion of the target, go to the configurable joint, angular x limit spring and change the damper to increase to make it a smaller change in velocity and less if u want a faster velocity.")]
    private ConfigurableJoint joint;
    [Tooltip("If you want to not use a timer to set it back up, set this to 0.")]
    [SerializeField] private float timerToBringBackUpPanel = 1.0f;
    private int angleToStopAt = 70;
    private float deltaTime = 0;
    private Vector3 savedJointOriginalRot;
    private Rigidbody rb;
    private bool hasStarted;
    private bool hasBeenHit = false;
    private void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        hasStarted = true;
        savedJointOriginalRot = transform.eulerAngles;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (hasStarted)
        {
            rb.angularVelocity = Vector3.zero;
            joint.angularXMotion = ConfigurableJointMotion.Limited;
            hasStarted = false;
        }
        if (transform.eulerAngles.x > 200 && transform.eulerAngles.x < angleToStopAt)
        {
            rb.angularVelocity = Vector3.zero;
            transform.eulerAngles = new Vector3(angleToStopAt, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        if (!hasBeenHit && transform.eulerAngles.x != 0)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        if (timerToBringBackUpPanel != 0 && hasBeenHit)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime >= timerToBringBackUpPanel)
            {
                joint.angularXMotion = ConfigurableJointMotion.Locked;
                deltaTime = 0;
                rb.angularVelocity = Vector3.zero;
                transform.eulerAngles = savedJointOriginalRot;
                hasStarted = true;
                hasBeenHit = false;
            }
        }
    }
    private void OnCollisionEnter()
    {
        hasBeenHit = true;
    }
}
