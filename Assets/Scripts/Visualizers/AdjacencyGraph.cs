using System;
using System.Collections.Generic;
using System.Linq;

public class AdjacencyGraph<T>
{
    public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new();

    public void Clear()
    {
        AdjacencyList.Clear();
    }

    public void AddVertex(T vertex)
    {
        AdjacencyList.TryAdd(vertex, new HashSet<T>());
    }

    public void AddEdge(T start, T end)
    {
        if (AdjacencyList.TryGetValue(start, out var list))
            list.Add(end);
        else
            AdjacencyList.Add(start, new HashSet<T> { end, });
    }

    public void AddUndirectedEdge(T start, T end)
    {
        AddEdge(start, end);
        AddEdge(end, start);
    }

    public List<T> Dijkstra(T start, T end)
    {
        var distances = new Dictionary<T, float>();
        var cameFrom = new Dictionary<T, T>();
        var visited = new HashSet<T>();

        distances.Add(start, 0);

        while (distances.Count > 0)
        {
            var (current, score) = distances.OrderBy(pair => pair.Value).First();
            distances.Remove(current);
            visited.Add(current);

            if (current.Equals(end))
                return GeneratePath(cameFrom, current);

            foreach (var neighbor in AdjacencyList[current])
            {
                if (visited.Contains(neighbor))
                    continue;

                var newDistance = score + 1;
                if (distances.ContainsKey(neighbor) && newDistance >= distances[neighbor])
                    continue;

                distances[neighbor] = newDistance;
                cameFrom[neighbor] = current;
            }
        }

        throw new Exception($"No path found between {start} and {end}");
    }

    static List<T> GeneratePath(IReadOnlyDictionary<T, T> cameFrom, T endState)
    {
        var path = new List<T>();
        var parent = endState;
        while (parent != null && cameFrom.ContainsKey(parent))
        {
            path.Add(parent);
            parent = cameFrom[parent];
        }

        path.Reverse();
        return path;
    }
}