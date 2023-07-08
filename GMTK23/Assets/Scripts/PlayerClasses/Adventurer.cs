using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    public int adventurerLevel = 1;

    private float levelModBase = 2f;
    public virtual float CalculateMultiplier(WordType wordType) { return 1f; }

    public float ParseWordOrPhrase(PhraseObject phrase)
    {
        float multiplier = CalculateMultiplier(phrase.wordType);
        float levelMod = 1f;
        if (phrase.useLevelModifier)
        {
            levelMod = levelModBase - Mathf.Abs(adventurerLevel - phrase.idealLevel) * (levelModBase / 100f);
        }
        return phrase.impact * multiplier * levelMod;
    }
}

public class Brawler : Adventurer
{
    public override float CalculateMultiplier(WordType wordType)
    {
        switch (wordType) 
        {
            case WordType.ActionOriented: return 2f;
            case WordType.Intelligent: return 0f;
            case WordType.Helpful: return -2f;
            case WordType.Careful: return -1f;
            case WordType.Magical: return 1f;
            case WordType.Greedy: return 1f;
            case WordType.Neutral: return 1f;
        }
        return 1f;
    }
}

public class Mage : Adventurer
{
    public override float CalculateMultiplier(WordType wordType)
    {
        switch (wordType)
        {
            case WordType.ActionOriented: return 1f;
            case WordType.Intelligent: return 2f;
            case WordType.Helpful: return 1f;
            case WordType.Careful: return -2f;
            case WordType.Magical: return 2f;
            case WordType.Greedy: return 0f;
            case WordType.Neutral: return 1f;
        }
        return 1f;
    }
}

public class Support : Adventurer
{
    public override float CalculateMultiplier(WordType wordType)
    {
        switch (wordType)
        {
            case WordType.ActionOriented: return -2f;
            case WordType.Intelligent: return 1f;
            case WordType.Helpful: return 2f;
            case WordType.Careful: return 1f;
            case WordType.Magical: return 1f;
            case WordType.Greedy: return -2f;
            case WordType.Neutral: return 1f;
        }
        return 1f;
    }
}

public class Rogue : Adventurer
{
    public override float CalculateMultiplier(WordType wordType)
    {
        switch (wordType)
        {
            case WordType.ActionOriented: return -2f;
            case WordType.Intelligent: return 1f;
            case WordType.Helpful: return 0f;
            case WordType.Careful: return 2f;
            case WordType.Magical: return -1f;
            case WordType.Greedy: return 2f;
            case WordType.Neutral: return 1f;
        }
        return 1f;
    }
}