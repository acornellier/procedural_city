using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Visualizer : MonoBehaviour
{
    [SerializeField] LSystemGenerator lSystem;
    [SerializeField] Vector2Int lengths = new(1, 3);

    [NonSerialized] public int seed;

    const float _angle = 90;
    protected GameObject city;
    protected readonly RoadNetwork roadNetwork = new();

    public abstract void Visualize();

    protected void BuildRoadNetwork()
    {
        Random.InitState(seed);
        Utilities.SafeDestroy(city);
        city = new GameObject("City");
        roadNetwork.Clear();

        var sequence = lSystem.GenerateSentence();

        var savePoints = new Stack<AgentParameters>();
        var currentPosition = Vector2Int.zero;
        var direction = Vector2Int.up;
        var length = 0;

        foreach (var encoding in sequence.Select(letter => (EncodingLetters)letter))
        {
            switch (encoding)
            {
                case EncodingLetters.Save:
                    savePoints.Push(
                        new AgentParameters
                            { position = currentPosition, direction = direction, length = length }
                    );
                    break;
                case EncodingLetters.Load:
                    if (savePoints.Count <= 0)
                        throw new Exception("Attempting to load with no saved points in stack");

                    var agentParameter = savePoints.Pop();
                    currentPosition = agentParameter.position;
                    direction = agentParameter.direction;
                    break;
                case EncodingLetters.Draw:
                    length = Random.Range(lengths.x, lengths.y);
                    var endPosition = currentPosition + direction * length;
                    roadNetwork.AddRoad(currentPosition, direction, length);
                    currentPosition = endPosition;
                    break;
                case EncodingLetters.TurnLeft:
                case EncodingLetters.TurnRight:
                    var rotatedAngle = encoding == EncodingLetters.TurnLeft ? _angle : -_angle;
                    var newDirection = Quaternion.AngleAxis(rotatedAngle, Vector3.forward) * (Vector2)direction;
                    direction = Vector2Int.RoundToInt(newDirection);
                    break;
                case EncodingLetters.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}