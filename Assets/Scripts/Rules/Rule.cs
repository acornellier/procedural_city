using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Rule", menuName = "ProceduralCity/Rule", order = 0)]
public class Rule : ScriptableObject
{
    public string letter;

    [SerializeField] string[] results;

    public string GetResult()
    {
        return results[Random.Range(0, results.Length)];
    }
}