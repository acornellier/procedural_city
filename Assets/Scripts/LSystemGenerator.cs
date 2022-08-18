using System.Text;
using UnityEngine;

public class LSystemGenerator : MonoBehaviour
{
    [SerializeField] Rule[] _rules;
    [SerializeField] string _rootSentence;
    [SerializeField] [Range(0, 50)] int _iterationLimit;
    [SerializeField] [Range(0, 1)] float _chanceToIgnoreRule = 0.3f;
    [SerializeField] int _maxTotalLength = 10000;

    int _totalLength = 0;

    public string GenerateSentence(string word = null)
    {
        _totalLength = 0;
        word ??= _rootSentence;

        return GrowRecursive(word);
    }

    string GrowRecursive(string word, int iterationIndex = 0)
    {
        if (iterationIndex >= _iterationLimit)
            return word;

        var newWord = new StringBuilder();
        foreach (var c in word)
        {
            _totalLength += 1;
            if (_totalLength >= _maxTotalLength)
                break;

            newWord.Append(c);
            ProcessRulesRecursively(newWord, c, iterationIndex);
        }

        return newWord.ToString();
    }

    void ProcessRulesRecursively(StringBuilder newWord, char c, int iterationIndex)
    {
        foreach (var rule in _rules)
        {
            if (rule._letter == c.ToString() &&
                (_chanceToIgnoreRule == 0 || iterationIndex <= 1 || Random.value > _chanceToIgnoreRule))
                newWord.Append(GrowRecursive(rule.GetResult(), iterationIndex + 1));
        }
    }
}