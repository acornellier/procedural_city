using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField] Marker[] neighbors;
    public bool isEntrance;

    public bool IsExit => neighbors.Length == 0;
    public IEnumerable<Marker> Neighbors => neighbors;

    void OnDrawGizmos()
    {
        if (Selection.activeObject != gameObject)
            return;

        foreach (var neighbor in neighbors)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
}