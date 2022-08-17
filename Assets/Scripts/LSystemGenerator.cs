using System.Text;
using UnityEngine;

public class LSystemGenerator : MonoBehaviour
{
    [SerializeField] Rule[] rules;
    [SerializeField] string rootSentence;

    [SerializeField] [Range(0, 10)] public int iterationLimit;

    public string GenerateSentence(string word = null)
    {
        word ??= rootSentence;

        return GrowRecursive(word);
    }

    string GrowRecursive(string word, int iterationIndex = 0)
    {
        if (iterationIndex >= iterationLimit)
            return word;

        var newWord = new StringBuilder();
        foreach (var c in word)
        {
            newWord.Append(c);
            ProcessRulesRecursively(newWord, c, iterationIndex);
        }

        return newWord.ToString();
    }

    void ProcessRulesRecursively(StringBuilder newWord, char c, int iterationIndex)
    {
        foreach (var rule in rules)
        {
            if (rule.letter == c.ToString())
                newWord.Append(GrowRecursive(rule.GetResult(), iterationIndex + 1));
        }
    }
}