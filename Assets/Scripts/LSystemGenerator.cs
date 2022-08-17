using System.Text;
using UnityEngine;

public class LSystemGenerator : MonoBehaviour
{
    [SerializeField] Rule[] rules;
    [SerializeField] string rootSentence;
    [SerializeField] [Range(0, 50)] int iterationLimit;
    [SerializeField] [Range(0, 1)] float chanceToIgnoreRule = 0.3f;
    [SerializeField] int maxTotalLength = 10000;

    int _totalLength = 0;

    public string GenerateSentence(string word = null)
    {
        _totalLength = 0;
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
            _totalLength += 1;
            if (_totalLength >= maxTotalLength)
                break;

            newWord.Append(c);
            ProcessRulesRecursively(newWord, c, iterationIndex);
        }

        return newWord.ToString();
    }

    void ProcessRulesRecursively(StringBuilder newWord, char c, int iterationIndex)
    {
        foreach (var rule in rules)
        {
            if (rule.letter == c.ToString() &&
                (chanceToIgnoreRule == 0 || iterationIndex <= 1 || Random.value > chanceToIgnoreRule))
                newWord.Append(GrowRecursive(rule.GetResult(), iterationIndex + 1));
        }
    }
}