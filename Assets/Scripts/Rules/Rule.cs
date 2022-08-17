using UnityEngine;

[CreateAssetMenu(fileName = "Rule", menuName = "ProceduralCity/Rule", order = 0)]
public class Rule : ScriptableObject
{
    public string letter;
    public string[] results;

    public string GetResult()
    {
        return results[0];
    }
}