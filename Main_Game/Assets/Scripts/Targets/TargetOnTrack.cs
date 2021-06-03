using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetOnTrack : MonoBehaviour
{
    //Direction the target is moving
    public enum Direction
    {
        REVERSE,
        FORWARD
    }
    //The way the target is moving
    public enum Movement
    {
        OtherDirection,
        ContinueInLine
    }
    [SerializeField] private bool canMove = true;
    [Tooltip("All the gameobjects on the trail line.")]
    [SerializeField] private List<Transform> a_trails = null;
    [SerializeField] private float speed = 1;
    [SerializeField] private Direction CurrentDirection = Direction.FORWARD;
    [Tooltip("When it reaches the last rail, will it go the other direction or continue back to the original point (if its close to the end point).")]
    [SerializeField] private Movement wayToMove = Movement.OtherDirection;
    [Tooltip("The place the target goes to first. It goes by index so it will start from 0 onwards.")]
    [SerializeField] private int currentTarget = 0;

    private void Update()
    {
        //Quick out of the class if there isnt any trails and the target does not move.
        if (a_trails != null && canMove)
        {
            if (IsAtCurrentTarget())
            {
                GetNextIndex();
                //If the index is beyond the amount within the list of objects.
                if (currentTarget >= a_trails.Count || currentTarget <= -1)
                {
                    if (wayToMove == Movement.ContinueInLine)
                        IsEndTargetGoal();
                    else if (wayToMove == Movement.OtherDirection)
                    {
                        ChangeDirections();
                        GetNextIndex();
                    }
                }
            }
            MoveTowardsTarget();
        }
    }
    //Finds the distance between one object compared to another through the distance square rule
    private float DistanceSqud(Vector3 from, Vector3 to)
    {
        Vector3 fromTo = to - from;
        float dissqrd = Vector3.Dot(fromTo, fromTo);
        return dissqrd;
    }
    //Checks if the distance between the target and the position of the empty gameobject is exactly the same or very close to.
    private bool IsAtCurrentTarget()
    {
        return DistanceSqud(transform.position, a_trails[currentTarget].position) <= float.Epsilon;
    }
    //Increments or decreases the index of the target direction.
    private void GetNextIndex()
    {
        switch (CurrentDirection)
        {
            case Direction.FORWARD:
                {
                    currentTarget++;
                    break;
                }
            case Direction.REVERSE:
                {
                   currentTarget --;
                    break;
                }
        }
    }
    //Changes the directions of the target.
    private void ChangeDirections() 
    {
        switch (CurrentDirection)
        {
            case Direction.REVERSE:
                 CurrentDirection = Direction.FORWARD;
                break;
            case Direction.FORWARD:
                CurrentDirection = Direction.REVERSE;
                break;
        }
    }
    //Sets the end of the goal to continuously move in a circular motion.
    private void IsEndTargetGoal()
    {
        switch (CurrentDirection)
        {
            case Direction.REVERSE:
                currentTarget = a_trails.Count - 1;
                break;
            case Direction.FORWARD:
                currentTarget = 0;
                break;
        }
    }
    //This constantly movestowards the target in the index currentTarget.
    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position,
         a_trails[currentTarget].position, speed * Time.deltaTime);
    }
}
