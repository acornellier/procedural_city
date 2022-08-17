using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleVisualizer))]
public class SimpleVisualizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var visualizer = (SimpleVisualizer)target;
        if (GUILayout.Button("Visualize"))
            visualizer.Visualize();
    }
}