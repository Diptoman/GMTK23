using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PhraseObject", menuName = "Word or Phrase")]
public class PhraseObject : ScriptableObject
{
    public string word;
    public WordType wordType;
    [Range(1f, 3f)]
    public float impact = 1f;
    public bool useLevelModifier = false;
    [ShowIf("@this.useLevelModifier == true")]
    [Range(1, 100)]
    public int idealLevel = 1;
}