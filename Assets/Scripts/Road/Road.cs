using UnityEngine;

public class Road : MonoBehaviour
{
    public Marker[] CarMarkers { get; private set; }

    void Awake()
    {
        CarMarkers = GetComponentsInChildren<Marker>();
    }
}