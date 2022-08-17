using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleVisualizer : MonoBehaviour
{
    [SerializeField] LSystemGenerator lSystem;
    [SerializeField] GameObject prefab;
    [SerializeField] Material lineMaterial;
    [SerializeField] float angle = 90;
    [SerializeField] int startingLength = 8;

    GameObject _city;
    readonly List<Vector2> _positions = new();
    int _length;

    int Length
    {
        get => _length > 0 ? _length : 1;
        set => _length = value;
    }

    public void Visualize()
    {
        Utilities.SafeDestroy(_city);
        VisualizeSequence(lSystem.GenerateSentence());
    }

    void VisualizeSequence(string sequence)
    {
        _city = new GameObject("City");
        _length = startingLength;
        _positions.Clear();

        var savePoints = new Stack<AgentParameters>();
        var currentPosition = Vector2.zero;
        var direction = Vector2.up;

        _positions.Add(currentPosition);
        foreach (var encoding in sequence.Select(letter => (EncodingLetters)letter))
        {
            switch (encoding)
            {
                case EncodingLetters.Save:
                    savePoints.Push(
                        new AgentParameters
                            { position = currentPosition, direction = direction, length = Length, }
                    );
                    break;
                case EncodingLetters.Load:
                    if (savePoints.Count <= 0)
                        throw new Exception(
                            "Attempting to load with no saved points in stack"
                        );

                    var agentParameter = savePoints.Pop();
                    currentPosition = agentParameter.position;
                    direction = agentParameter.direction;
                    Length = agentParameter.length;
                    break;
                case EncodingLetters.Draw:
                    var tempPosition = currentPosition;
                    currentPosition += direction * Length;
                    DrawLine(tempPosition, currentPosition, Color.red);
                    Length -= 1;
                    _positions.Add(currentPosition);
                    break;
                case EncodingLetters.TurnLeft:
                    direction = Quaternion.AngleAxis(angle, Vector3.forward) * direction;
                    break;
                case EncodingLetters.TurnRight:
                    direction = Quaternion.AngleAxis(-angle, Vector3.forward) * direction;
                    break;
                case EncodingLetters.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        foreach (var position in _positions)
        {
            Instantiate(
                prefab,
                position,
                Quaternion.identity,
                _city.transform
            );
        }
    }

    void DrawLine(Vector2 startPosition, Vector2 endPosition, Color color)
    {
        var line = new GameObject("line")
        {
            transform = { parent = _city.transform, position = startPosition, },
        };
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }

    enum EncodingLetters
    {
        Unknown = '1',
        Save = '[',
        Load = ']',
        Draw = 'F',
        TurnLeft = '-',
        TurnRight = '+',
    }
}