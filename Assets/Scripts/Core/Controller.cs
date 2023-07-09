using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class Controller : SerializedMonoBehaviour
{
    public List<QuestLine> questStructures = new List<QuestLine>();

    [OdinSerialize]
    public Dictionary<Mood, float> moodCeilingsPerPhrase = new Dictionary<Mood, float>();
    [OdinSerialize]
    public Dictionary<Mood, float> moodCeilingsOverall = new Dictionary<Mood, float>();

    [Title("Scene References")]
    public GameObject canvas;

    [Title("Prefab")]
    public GameObject adventurerPrefab;
    private GameObject currentAdventurer;
    private float currentMoodScore = 0f;

    [Title("Autofills")]
    public List<PhraseObject> things = new List<PhraseObject>();
    public List<PhraseObject> adjectives = new List<PhraseObject>();
    public List<PhraseObject> locations = new List<PhraseObject>();
    public List<PhraseObject> verbs = new List<PhraseObject>();

    //Metagame
    public float dayTimerMax = 120f;
    private int currentDay = 0;
    private int dayTaskNum = 4;
    private int dayTasksCompleted = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        InitData();
    }

    private void Start()
    {
        StartDay();
    }

    public void StartDay()
    {
        SpawnAdventurer();
        currentDay++;
        dayTimerMax = dayTimerMax + (currentDay - 1) * 10f; //New day timer
        canvas.GetComponent<UIController>().BeginDay(dayTimerMax);
    }

    public void SpawnAdventurer()
    {
        CharacterClass selectedClass = (CharacterClass)UnityEngine.Random.Range(0, 4);
        currentAdventurer = Instantiate(adventurerPrefab);
        currentAdventurer.GetComponent<CharacterRandomizer>().RandomizeCharacter(selectedClass);
        currentAdventurer.GetComponent<Adventurer>().characterClass = selectedClass;
        currentAdventurer.GetComponent<Adventurer>().controller = this.gameObject;
    }

    public void BeginDialogueSequence()
    {
        canvas.GetComponent<UIController>().BeginDialogue();
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

    public List<PhraseObject> PickOptions(WordClassification wordClass, int amount)
    {
        List<PhraseObject> wordList = new List<PhraseObject>();
        List<PhraseObject> selectedWordList = new List<PhraseObject>();
        switch (wordClass)
        {
            case WordClassification.Adjective: wordList.AddRange(adjectives); break;
            case WordClassification.Location: wordList.AddRange(locations); break;
            case WordClassification.Thing: wordList.AddRange(things); break;
            case WordClassification.Things: wordList.AddRange(things); break;
        }

        for(int i=0; i<amount; i++)
        {
            int randomIndex = Random.Range(0, wordList.Count - 1);
            Debug.Log("Wordlist Count" + wordList.Count);
            selectedWordList.Add(wordList[randomIndex]);
            wordList.RemoveAt(randomIndex);
        }

        return selectedWordList;
    }

    public void ParseCurrentPhrase(PhraseObject phrase)
    {
        float moodScore = currentAdventurer.GetComponent<Adventurer>().ParseWordOrPhrase(phrase);
        currentMoodScore += moodScore;
        Mood moodFromWord = ParseMood(moodCeilingsPerPhrase, moodScore);
        Debug.Log("Mood from this: " + moodScore + " with mood " + moodFromWord);
        canvas.GetComponent<UIController>().ShowMoodIndicator(moodFromWord);
    }

    private Mood ParseMood(Dictionary<Mood, float> moodMap, float moodAmount)
    {
        float currentCeiling = -999f;
        foreach (KeyValuePair<Mood, float> mood in moodMap)
        {
            float nextCeiling = mood.Value;
            if (moodAmount > currentCeiling && moodAmount <= nextCeiling)
            {
                return mood.Key;
            }
            currentCeiling = nextCeiling;
        }

        return Mood.Ecstatic;
    }

    public void DecideQuestResult()
    {
        Mood finalMood = ParseMood(moodCeilingsOverall, currentMoodScore);
        canvas.GetComponent<UIController>().ShowMoodIndicator(finalMood, 2);
        Debug.Log("Final mood is " + finalMood);
        currentMoodScore = 0f;

        if (finalMood != Mood.Angry)
        {
            //Clean up this round
            Debug.Log("Fugging Off");
            currentAdventurer.GetComponent<Adventurer>().FuggOff();
        }
        else
        {
            //End game
            Debug.Log("Murdering you");
            currentAdventurer.GetComponent<Adventurer>().BitchSlap();
        }

        canvas.GetComponent<UIController>().EndDialogue();
    }

    public void EndRound()
    {
        if (!isGameOver)
        {
            SpawnAdventurer();
        }
    }

    public void OutOfTime()
    {
        isGameOver = true;
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

public enum Mood
{
    Angry,
    Confused,
    Neutral,
    Happy,
    Ecstatic
}

public enum EndReason
{
    OutOfTime,
    KilledByWarrior,
    KilledByMage,
    KilledBySupport,
    KilledByRogue
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