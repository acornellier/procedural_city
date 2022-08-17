using System.Collections.Generic;
using UnityEngine;

public class RoadNetwork
{
    public Dictionary<Vector2Int, HashSet<Vector2Int>> AdjacencyList { get; } = new();

    public void Clear()
    {
        AdjacencyList.Clear();
    }

    public void AddRoad(Vector2Int start, Vector2Int direction, int length)
    {
        var currentPosition = start;
        for (var i = 0; i < length; ++i)
        {
            var end = currentPosition + direction;
            AddEdge(currentPosition, end);
            AddEdge(end, currentPosition);
            currentPosition = end;
        }
    }

    void AddEdge(Vector2Int start, Vector2Int end)
    {
        if (AdjacencyList.TryGetValue(start, out var list))
            list.Add(end);
        else
            AdjacencyList.Add(start, new HashSet<Vector2Int> { end });
    }
}