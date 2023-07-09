using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PhraseObject", menuName = "Word or Phrase")]
public class PhraseObject : SerializedScriptableObject
{
    public string word;
    [Tooltip("If the word has an alternate use, eg. plural for nouns, or present tense for verb")]
    public bool hasAlternateUsage = false;
    [ShowIf("@this.hasAlternateUsage == true")]
    public string alternateWord;
    public WordModifier wordModifier;
    [Range(1f, 4f)]
    public float impact = 1f;

    void OnValidate()
    {
        if (Application.isPlaying)
            return;

        var isObjectNameMatching = word.ToLower().Contains(this.name.ToLower());
        var isAltMatching = alternateWord.ToLower().Contains(this.name.ToLower());

        if (!isObjectNameMatching && !isAltMatching)
        {
            Debug.LogWarning("Words for scriptable object " + this.name + " don't match!");
        }
    }
}
