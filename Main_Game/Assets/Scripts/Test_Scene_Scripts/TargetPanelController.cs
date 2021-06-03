using UnityEngine;

public class TargetPanelController : MonoBehaviour
{
    public enum Direction
    {
        LEFT,
        RIGHT
    }

    public enum Axis
    {
        X,
        Y,
        Z
    }

    public GameObject PanelJoint;
    public Direction CurrentDirection;
    public Axis CurrentAxis;
    public float LeftLimit;
    public float RightLimit;
    [Range(0.0f, 10.0f)] public float Speed = 1.0f;
    public bool IsSliding;

    public void StartSliding(Direction a_Direction)
    {
        IsSliding = true;
    }

    private void Update()
    {
        if (!IsSliding)
        {
            return;
        }

        switch (CurrentDirection)
        {
            case Direction.LEFT:
                {
                    PanelJoint.transform.Translate(Vector3.left * Time.deltaTime * Speed, Space.Self);

                    if (Vector3.SqrMagnitude( PanelJoint.transform.position - gameObject.transform.position) > LeftLimit * LeftLimit)
                    {
                        CurrentDirection = Direction.RIGHT;
                    }

                    break;
                }
            case Direction.RIGHT:
                {
                    PanelJoint.transform.Translate(Vector3.right * Time.deltaTime * Speed, Space.Self);
                    float value = Vector3.SqrMagnitude(PanelJoint.transform.position - gameObject.transform.position);
                    if ( value > RightLimit * RightLimit)
                    {
                        CurrentDirection = Direction.LEFT;
                    }

                    break;
                }
        }
    }
}
