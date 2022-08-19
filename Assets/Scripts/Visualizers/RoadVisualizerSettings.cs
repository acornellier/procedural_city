using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "RoadVisualizerSettings",
    menuName = "ProceduralCity/RoadVisualizerSettings",
    order = 0
)]
public class RoadVisualizerSettings : ScriptableObject
{
    public Roads roads;
    public GameObject house;
    public GameObject grass;
    public CarAi car;

    [Serializable]
    public class Roads
    {
        public Road straight;
        public Road deadEnd;
        public Road corner;
        public Road threeWay;
        public Road fourWay;
    }
}