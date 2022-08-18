using UnityEditor;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField] bool _openForConnections;
    [SerializeField] Marker[] _neighbors;

    public Marker[] Neighbors => _neighbors;

    void OnDrawGizmos()
    {
        if (Selection.activeObject != gameObject)
            return;

        foreach (var neighbor in _neighbors)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
}