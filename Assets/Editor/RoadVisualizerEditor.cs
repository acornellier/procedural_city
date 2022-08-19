using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoadVisualizer))]
public class RoadVisualizerEditor : VisualizerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var visualizer = (RoadVisualizer)target;

        if (GUILayout.Button("Spawn Car"))
            visualizer.SpawnCar();
    }
}