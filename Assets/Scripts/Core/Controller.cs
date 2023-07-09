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
    public float dayTimerMax = 100f;
    [HideInInspector]
    public float dayTimerCurrentMax;
    [HideInInspector]
    public int currentDay = 0;
    public int dayTaskNum = 3;
    [HideInInspector]
    public float currentDayTaskNum;
    [HideInInspector]
    public int dayTasksCompleted = 0;
    private bool isGameOver = false;
    public Dictionary<Mood, int> moodTracker = new Dictionary<Mood, int>(); 

    private void Awake()
    {
        Application.targetFrameRate = 60;
        InitData();
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(ResetGame(.75f));
            }
        }
    }

    private IEnumerator ResetGame(float startDelay)
    {
        canvas.GetComponent<UIController>().ResetGame();
        Destroy(currentAdventurer.gameObject);
        yield return new WaitForSeconds(startDelay);
        StartGame();
    }

    public void StartGame()
    {
        dayTimerCurrentMax = dayTimerMax;
        currentDay = 0;
        currentDayTaskNum = dayTaskNum;

        StartDay();

        //Reset mood tracker
        moodTracker[Mood.Angry] = 0;
        moodTracker[Mood.Confused] = 0;
        moodTracker[Mood.Neutral] = 0;
        moodTracker[Mood.Happy] = 0;
        moodTracker[Mood.Ecstatic] = 0;
    }

    public void StartDay()
    {
        SpawnAdventurer();
        dayTasksCompleted = 0;
        currentDay++;
        dayTimerCurrentMax = dayTimerMax + currentDay * 20f; //New day timer
        currentDayTaskNum++;
        canvas.GetComponent<UIController>().BeginDay(dayTimerCurrentMax);
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

        //Set moods
        moodTracker.Add(Mood.Angry, 0);
        moodTracker.Add(Mood.Confused, 0);
        moodTracker.Add(Mood.Neutral, 0);
        moodTracker.Add(Mood.Happy, 0);
        moodTracker.Add(Mood.Ecstatic, 0);
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
            currentAdventurer.GetComponent<Adventurer>().FuggOff();
            dayTasksCompleted += 1;
        }
        else
        {
            //End game
            currentAdventurer.GetComponent<Adventurer>().BitchSlap();
        }

        //Add to mood tracker
        moodTracker[finalMood] = moodTracker[finalMood] + 1;

        canvas.GetComponent<UIController>().EndDialogue();
    }

    public void EndRound()
    {
        if (!isGameOver)
        {
            if (dayTasksCompleted == currentDayTaskNum)
            {
                StartDay();
            }
            else
            {
                SpawnAdventurer();
            }
        }
    }

    public void GameOver(EndReason reason)
    {
        isGameOver = true;
        canvas.GetComponent<UIController>().GameOver(reason);
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