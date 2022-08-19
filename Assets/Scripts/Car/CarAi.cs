using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarAi : MonoBehaviour
{
    [SerializeField] CarController carController;

    [SerializeField] float arriveDistance = 0.3f;
    [SerializeField] float lastPointArriveDistance = 0.1f;
    [SerializeField] float steeringStrength = 45f;
    [SerializeField] float steeringEpsilon = 5f;

    [SerializeField] List<Vector2> path = new();
    [ReadOnly] [SerializeField] int pathIndex;
    [ReadOnly] [SerializeField] bool stopped;

    CarAiDirector _director;

    void Awake()
    {
        stopped = true;
    }

    void Update()
    {
        if (stopped && _director != null) GetNewPath();

        CheckIfArrived();
        Drive();
    }

    void OnDrawGizmos()
    {
        if (Selection.activeGameObject != gameObject) return;

        for (var i = 0; i < path.Count; ++i)
        {
            Gizmos.color = i == pathIndex ? Color.magenta : i < pathIndex ? Color.green : Color.red;

            Gizmos.DrawSphere(path[i], .05f);

            if (i > 0)
                Gizmos.DrawLine(path[i - 1], path[i]);
        }
    }

    public void SetDirector(CarAiDirector director)
    {
        _director = director;
    }

    void GetNewPath()
    {
        path = _director.GetRandomPath(transform.position);
        if (path.Count <= 1)
        {
            stopped = true;
            return;
        }

        transform.position = path[0];
        var relativePoint = transform.InverseTransformPoint(path[1]);
        var angle = Mathf.Atan2(relativePoint.x, relativePoint.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        pathIndex = 1;
        stopped = false;
    }

    void CheckIfArrived()
    {
        if (stopped) return;

        var distanceToCheck =
            pathIndex == path.Count - 1 ? lastPointArriveDistance : arriveDistance;
        var distanceFromPoint = Vector2.Distance(transform.position, path[pathIndex]);
        if (distanceFromPoint < distanceToCheck)
            SetNextTargetIndex();
    }

    void SetNextTargetIndex()
    {
        ++pathIndex;
        if (pathIndex < path.Count) return;

        stopped = true;
        path.Clear();
    }

    void Drive()
    {
        if (stopped)
        {
            carController.SetInput(0, 0);
            return;
        }

        var direction = (Vector3)path[pathIndex] - transform.position;
        var angleDiff = Vector2.SignedAngle(transform.up, direction);
        var turnInput = Mathf.Abs(angleDiff) < steeringEpsilon
            ? 0
            : -angleDiff / steeringStrength;
        carController.SetInput(turnInput, 1);
    }
}