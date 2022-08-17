using UnityEditor;
using UnityEngine;

public class VisualizerEditor : Editor
{
    const int _minSeed = 0;
    const int _maxSeed = 1000;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var visualizer = (Visualizer)target;

        visualizer.seed = EditorGUILayout.IntSlider("Seed", visualizer.seed, _minSeed, _maxSeed);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Visualize"))
            visualizer.Visualize();

        if (GUILayout.Button("Randomize"))
        {
            visualizer.seed = Random.Range(_minSeed, _maxSeed);
            visualizer.Visualize();
        }

        GUILayout.EndHorizontal();
    }
}