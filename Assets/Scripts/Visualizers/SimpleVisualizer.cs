﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleVisualizer : Visualizer
{
    [SerializeField] GameObject _prefab;
    [SerializeField] Material _lineMaterial;

    public override void Visualize()
    {
        BuildRoadNetwork();
        foreach (var (vertex, edges) in roadNetwork.AdjacencyList)
        {
            DrawPosition(vertex);
            foreach (var edge in edges)
            {
                DrawConnection(vertex, edge);
            }
        }
    }

    void DrawConnection(Vector2 startPosition, Vector2 endPosition)
    {
        var line = new GameObject("line")
            { transform = { parent = city.transform, position = startPosition } };
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = _lineMaterial;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }

    void DrawPosition(Vector2 position)
    {
        Instantiate(_prefab, position, Quaternion.identity, city.transform);
    }
}