using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfShot : MonoBehaviour
{
    public Transform RayTransform;
    public LineRenderer RayRenderer;

    public Material Broad;
    public Material Hammer;
    public Material Water;

    public void ShowLine( float a_Speed, Arrow.ArrowType a_ArrowType )
    {
        RayRenderer.enabled = true;

        switch (a_ArrowType)
        {
            case Arrow.ArrowType.BROAD:
                {
                    RayRenderer.material = Broad;
                    break;
                }
            case Arrow.ArrowType.HAMMER:
                {
                    RayRenderer.material = Hammer;
                    break;
                }
            case Arrow.ArrowType.WATER:
                {
                    RayRenderer.material = Water;
                    break;
                }
            default:
                break;
        }

        ShowLine( RayRenderer, RayTransform.position, RayTransform.forward, a_Speed * 2800.0f, 0.0001f, 10.0f );
    }

    public void HideLine()
    {
        RayRenderer.enabled = false;
    }

    private void ShowLine(LineRenderer LineRenderer, Vector3 startPos, Vector3 direction, float speed, float timePerSegmentInSeconds, float maxTravelDistance)
    {
        var positions = new List<Vector3>();
        Vector3 currentPos = startPos;
        positions.Add(startPos);

        var traveledDistance = 0.0f;
        while (traveledDistance < maxTravelDistance)
        {
            traveledDistance += speed * timePerSegmentInSeconds;
            var hasHitSomething = TravelTrajectorySegment(currentPos, direction, speed, timePerSegmentInSeconds, positions);
            if (hasHitSomething)
            {
                break;
            }
            var lastPos = currentPos;
            currentPos = positions[positions.Count - 1];
            direction = currentPos - lastPos;
            direction.Normalize();
        }

        BuildTrajectoryLine(LineRenderer, positions);
    }

    private bool TravelTrajectorySegment(Vector3 startPos, Vector3 direction, float speed, float timePerSegmentInSeconds, List<Vector3> positions)
    {
        var newPos = startPos + direction * speed * timePerSegmentInSeconds + Physics.gravity * timePerSegmentInSeconds;

        RaycastHit hitInfo;
        var hasHitSomething = Physics.Linecast(startPos, newPos, out hitInfo);
        if (hasHitSomething)
        {
            newPos = hitInfo.point;
        }
        positions.Add(newPos);

        return hasHitSomething;
    }

    private void BuildTrajectoryLine(LineRenderer lineRenderer, List<Vector3> positions)
    {
        lineRenderer.positionCount = positions.Count;
        for (var i = 0; i < positions.Count; ++i)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }
    }
}
