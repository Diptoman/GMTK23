using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

[Serializable]
public class Controller : MonoBehaviour
{
    [OdinSerialize]
    public List<QuestLine> questStructures = new List<QuestLine>();

    [Title("Autofills")]
    public List<PhraseObject> things = new List<PhraseObject>();
    public List<PhraseObject> adjectives = new List<PhraseObject>();
    public List<PhraseObject> locations = new List<PhraseObject>();
    public List<PhraseObject> verbs = new List<PhraseObject>();

    private void Awake()
    {
        InitData();
    }

    private void InitData()
    {
        //Load Adjectives
        UnityEngine.Object[] adjectiveArray;
        adjectiveArray = Resources.LoadAll("ScriptableObjects/Adjective", typeof(PhraseObject));
        foreach (var data in adjectiveArray)
        {
            adjectives.Add((PhraseObject)data);
        }

        //Load Things
        UnityEngine.Object[] thingArray;
        thingArray = Resources.LoadAll("ScriptableObjects/Thing", typeof(PhraseObject));
        foreach (var data in thingArray)
        {
            things.Add((PhraseObject)data);
        }

        //Load locations
        UnityEngine.Object[] locationArray;
        locationArray = Resources.LoadAll("ScriptableObjects/Location", typeof(PhraseObject));
        foreach (var data in locationArray)
        {
            locations.Add((PhraseObject)data);
        }

        //Load verbs
        UnityEngine.Object[] verbArray;
        verbArray = Resources.LoadAll("ScriptableObjects/Verb", typeof(PhraseObject));
        foreach (var data in verbArray)
        {
            verbs.Add((PhraseObject)data);
        }
    }
}

public enum WordClassification
{
    Adjective,
    Location,
    Thing,
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