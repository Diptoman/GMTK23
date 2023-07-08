using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    public int adventurerLevel = 1;
    public CharacterClass characterClass;

    private float levelModBase = 2f;
    public virtual float CalculateMultiplier(WordType wordType) { return 1f; }

    public float ParseWordOrPhrase(PhraseObject phrase)
    {
        //Update with new calculator tomorrow
        float multiplier = CalculateMultiplier(phrase.wordType);
        float levelMod = 1f;
        if (phrase.useLevelModifier)
        {
            levelMod = levelModBase - Mathf.Abs(adventurerLevel - phrase.idealLevel) * (levelModBase / 100f);
        }
        return phrase.impact * multiplier * levelMod;
    }
}

public enum CharacterClass
{
    Brawler,
    Mage,
    Support,
    Rogue
}