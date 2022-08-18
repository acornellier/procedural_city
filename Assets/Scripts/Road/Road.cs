using UnityEngine;

public class Road : MonoBehaviour
{
    Marker[] _carMarkers;

    void Awake()
    {
        _carMarkers = GetComponentsInChildren<Marker>();
    }
    //
    // Marker GetClosestMarkTo(Vector2 position, bool isCorner = false)
    // {
    //     return _carMarkers
    //         .OrderBy(marker => Vector3.Distance(position, marker.transform.position))
    //         .First();
    // }
}