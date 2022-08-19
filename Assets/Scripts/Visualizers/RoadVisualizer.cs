using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadVisualizer : Visualizer
{
    [SerializeField] RoadVisualizerSettings settings;

    readonly Vector2Int[] _directions =
        { Vector2Int.up, Vector2Int.right, Vector2Int.left, Vector2Int.down, };

    CarAiDirector _carAiDirector;

    BoundsInt _bounds;
    Transform _grassParent;
    Transform _roadParent;
    Transform _carParent;
    Dictionary<Vector2Int, Road> _roadMap;

    void Start()
    {
        Visualize();
        SpawnCar();
    }

    public override void Visualize()
    {
        BuildRoadNetwork();
        Reset();

        foreach (var position in roadNetwork.AdjacencyList.Keys)
        {
            if (position.x < _bounds.xMin) _bounds.xMin = position.x - 1;
            else if (position.x > _bounds.xMax) _bounds.xMax = position.x + 2;
            if (position.y < _bounds.yMin) _bounds.yMin = position.y - 1;
            else if (position.y > _bounds.yMax) _bounds.yMax = position.y + 2;
        }

        // DrawGrass();
        DrawRoads();
        // DrawHouses(roadPositions);

        _carAiDirector = new CarAiDirector(roadNetwork, _roadMap);
    }

    public void SpawnCar()
    {
        for (var i = 0; i < 100; ++i)
        {
            var position = _roadMap.Keys.ToList()[Random.Range(0, _roadMap.Count)];
            var car = Instantiate(
                settings.car,
                (Vector2)position,
                Quaternion.identity,
                _carParent
            );
            car.SetDirector(_carAiDirector);
        }
    }

    void Reset()
    {
        _roadMap = new Dictionary<Vector2Int, Road>();
        _bounds = new BoundsInt();
        _grassParent = new GameObject("Grass").transform;
        _grassParent.SetParent(city.transform);
        _roadParent = new GameObject("Roads").transform;
        _roadParent.SetParent(city.transform);
        _carParent = new GameObject("Cars").transform;
        _carParent.SetParent(city.transform);
    }

    void DrawGrass()
    {
        for (var x = _bounds.xMin; x <= _bounds.xMax; ++x)
        {
            for (var y = _bounds.yMin; y <= _bounds.yMax; ++y)
            {
                var position = new Vector2Int(x, y);
                var obj = Instantiate(
                    settings.grass,
                    (Vector2)position,
                    Quaternion.identity,
                    _grassParent
                );
                // _roadMap[position] = obj;
            }
        }
    }

    void DrawRoads()
    {
        var visited = new HashSet<Vector2Int>();

        foreach (var (point, neighbors) in roadNetwork.AdjacencyList)
        {
            if (neighbors.Count is 0 or >= 5)
                throw new Exception("Must have between 1 and 4 neighbors");

            if (neighbors.Count == 4)
            {
                DrawRoad(settings.roads.fourWay, point, 0, visited);
                continue;
            }

            var down = false;
            var right = false;
            var up = false;
            var left = false;
            foreach (var direction in neighbors.Select(
                         neighbor =>
                             Vector2Int.RoundToInt(((Vector2)(neighbor - point)).normalized)
                     ))
            {
                if (direction == Vector2Int.down) down = true;
                else if (direction == Vector2Int.right) right = true;
                else if (direction == Vector2Int.up) up = true;
                else if (direction == Vector2Int.left) left = true;
            }

            if (neighbors.Count == 1)
            {
                var angle = down ? 0f
                    : right ? 90f
                    : up ? 180f
                    : 270f;
                DrawRoad(settings.roads.deadEnd, point, angle, visited);
            }
            else if (neighbors.Count == 2)
            {
                if (down && up)
                {
                    DrawRoad(settings.roads.straight, point, 0, visited);
                }
                else if (right && left)
                {
                    DrawRoad(settings.roads.straight, point, 90, visited);
                }
                else // corner
                {
                    var angle = down & right ? 0f
                        : right & up ? 90f
                        : up && left ? 180f
                        : 270f;
                    DrawRoad(settings.roads.corner, point, angle, visited);
                }
            }
            else if (neighbors.Count == 3)
            {
                var angle = !left ? 0f
                    : !down ? 90f
                    : !right ? 180f
                    : 270f;
                DrawRoad(settings.roads.threeWay, point, angle, visited);
            }
        }
    }

    void DrawRoad(Road road, Vector2Int position, float zAngle, ISet<Vector2Int> visited)
    {
        if (visited.Contains(position))
            return;

        var angle = Quaternion.Euler(0, 0, zAngle);
        var obj = Instantiate(road, (Vector2)position, angle, _roadParent);
        visited.Add(position);

        if (_roadMap.TryGetValue(position, out var tile))
            DestroyImmediate(tile);

        _roadMap[position] = obj;
    }

    void DrawHouses(ICollection<Vector2Int> roadPositions)
    {
        var houses = new HashSet<Vector2Int>();

        foreach (var roadPosition in roadPositions)
        {
            foreach (var direction in _directions)
            {
                var position = roadPosition + direction;
                if (roadPositions.Contains(position) || houses.Contains(position)) continue;

                if (Random.value < 0.5)
                    continue;

                houses.Add(position);
                var zAngle = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                var angle = Quaternion.Euler(0, 0, zAngle);
                Instantiate(settings.house, (Vector2)position, angle, city.transform);
            }
        }
    }
}