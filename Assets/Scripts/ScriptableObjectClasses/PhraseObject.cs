using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PhraseObject", menuName = "Word or Phrase")]
public class PhraseObject : ScriptableObject
{
    public string word;
    public bool hasPlural = false;
    [ShowIf("@this.hasPlural == true")]
    public string pluralWord;
    public WordModifier wordModifier;
    [Range(1f, 4f)]
    public float impact = 1f;
    public bool useLevelModifier = false;
    [ShowIf("@this.useLevelModifier == true")]
    [Range(1, 100)]
    public int idealLevel = 1;
    [ShowIf("@this.useLevelModifier == true")]
    [Range(0f, 4f)]
    public float maxLevelModifier = 4f;
}
