using UnityEngine;

[CreateAssetMenu(fileName = "Rule", menuName = "ProceduralCity/Rule", order = 0)]
public class Rule : ScriptableObject
{
    public string _letter;
    [SerializeField] string[] _results;

    public string GetResult()
    {
        return _results[Random.Range(0, _results.Length)];
    }
}