using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

[Serializable]
public class Controller : MonoBehaviour
{
    public List<ScriptableObject> things = new List<ScriptableObject>();
    public List<ScriptableObject> adjectives = new List<ScriptableObject>();
    public List<ScriptableObject> locations = new List<ScriptableObject>();

    [OdinSerialize]
    public List<QuestLine> questStructures = new List<QuestLine>();
}

public enum WordClassification
{
    Adjective,
    Location,
    Things
}

public enum WordType
{
    Neutral,
    ActionOriented,
    Intelligent,
    Helpful,
    Careful,
    Magical,
    Greedy
}

[Serializable]
public class PhrasePart
{
    public bool useExistingPhraseHere = false;
    [ShowIf("@this.useExistingPhraseHere == false")]
    public string phrase;
    [ShowIf("@this.useExistingPhraseHere == true")]
    public WordClassification wordClassification;
}

[Serializable]
public class QuestLine
{
    public List<PhrasePart> phrases = new List<PhrasePart>();
}