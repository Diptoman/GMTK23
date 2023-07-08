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
        float multiplier = 0f;
        switch (characterClass)
        {
            case CharacterClass.Warrior: multiplier = phrase.wordModifier.warriorModifier; break;
            case CharacterClass.Mage: multiplier = phrase.wordModifier.mageModifier; break;
            case CharacterClass.Support: multiplier = phrase.wordModifier.supportModifier; break;
            case CharacterClass.Rogue: multiplier = phrase.wordModifier.rogueModifier; break;
        }
        
        float levelMod = 1f;
        if (phrase.useLevelModifier)
        {
            levelMod = phrase.maxLevelModifier - Mathf.Abs(adventurerLevel - phrase.idealLevel) * (phrase.maxLevelModifier / 100f);
        }
        return ((phrase.impact * multiplier) + levelMod);
    }
}

public enum CharacterClass
{
    Warrior,
    Mage,
    Support,
    Rogue
}