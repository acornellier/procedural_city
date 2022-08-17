using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadVisualizer : Visualizer
{
    [SerializeField] Roads roads;

    public override void Visualize()
    {
        BuildRoadNetwork();

        var visited = new HashSet<Vector2Int>();

        foreach (var (point, neighbors) in roadNetwork.AdjacencyList)
        {
            if (neighbors.Count is 0 or >= 5) throw new Exception("Must have between 1 and 4 neighbors");

            if (neighbors.Count == 4)
            {
                DrawPoint(roads.fourWay, point, 0, visited);
                continue;
            }

            var down = false;
            var right = false;
            var up = false;
            var left = false;
            foreach (var direction in neighbors.Select(neighbor =>
                         Vector2Int.RoundToInt(((Vector2)(neighbor - point)).normalized)))
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
                DrawPoint(roads.deadEnd, point, angle, visited);
            }
            else if (neighbors.Count == 2)
            {
                if (down && up)
                {
                    DrawPoint(roads.straight, point, 0, visited);
                }
                else if (right && left)
                {
                    DrawPoint(roads.straight, point, 90, visited);
                }
                else // corner
                {
                    var angle = down & right ? 0f
                        : right & up ? 90f
                        : up && left ? 180f
                        : 270f;
                    DrawPoint(roads.corner, point, angle, visited);
                }
            }
            else if (neighbors.Count == 3)
            {
                var angle = !left ? 0f
                    : !down ? 90f
                    : !right ? 180f
                    : 270f;
                DrawPoint(roads.threeWay, point, angle, visited);
            }
        }
    }

    void DrawPoint(Sprite sprite, Vector2Int position, float zAngle, ISet<Vector2Int> visited)
    {
        if (visited.Contains(position))
            return;

        var angle = Quaternion.Euler(0, 0, zAngle);
        var obj = Instantiate(roads.prefab, (Vector2)position, angle, city.transform);
        obj.GetComponent<SpriteRenderer>().sprite = sprite;
        visited.Add(position);
    }

    [Serializable]
    class Roads
    {
        public GameObject prefab;
        public Sprite straight;
        public Sprite deadEnd;
        public Sprite corner;
        public Sprite threeWay;
        public Sprite fourWay;
    }
}