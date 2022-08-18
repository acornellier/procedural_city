using System;
using System.Collections.Generic;
using UnityEngine;

public class CarAi : MonoBehaviour
{
    [SerializeField] CarController _carController;

    [SerializeField] float _arriveDistance = 0.3f;
    [SerializeField] float _lastPointArriveDistance = 0.1f;
    [SerializeField] float _steeringStrength = 45f;
    [SerializeField] float _steeringEpsilon = 5f;

    [SerializeField] List<Vector2> _path = new();
    [ReadOnly] [SerializeField] int _pathIndex;
    [ReadOnly] [SerializeField] bool _stopped;

    void Awake()
    {
        _stopped = true;
    }

    void Update()
    {
        if (_stopped && _path.Count > 1) StartPath();

        CheckIfArrived();
        Drive();
    }

    void StartPath()
    {
        _pathIndex = 0;
        _stopped = false;
    }

    void OnDrawGizmos()
    {
        foreach (var point in _path)
        {
            Gizmos.DrawSphere(point, .05f);
        }
    }

    void CheckIfArrived()
    {
        if (_stopped) return;

        var distanceToCheck = _pathIndex == _path.Count - 1 ? _lastPointArriveDistance : _arriveDistance;
        var distanceFromPoint = Vector2.Distance(transform.position, _path[_pathIndex]);
        if (distanceFromPoint < distanceToCheck)
            SetNextTargetIndex();
    }

    void SetNextTargetIndex()
    {
        ++_pathIndex;
        if (_pathIndex < _path.Count) return;

        _stopped = true;
        _path.Clear();
    }

    void Drive()
    {
        if (_stopped)
        {
            _carController.SetInput(0, 0);
            return;
        }

        var direction = (Vector3)_path[_pathIndex] - transform.position;
        var angleDiff = Vector2.SignedAngle(transform.up, direction);
        var turnInput = Mathf.Abs(angleDiff) < _steeringEpsilon ? 0 : -angleDiff / _steeringStrength;
        _carController.SetInput(turnInput, 1);
    }
}