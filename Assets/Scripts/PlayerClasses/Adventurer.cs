using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    public int adventurerLevel = 1;
    public CharacterClass characterClass;
    public GameObject controller; //So much tightly coupled poor design for time's sake ;_;

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
        
        float levelMod = 0f;
        if (phrase.useLevelModifier)
        {
            levelMod = phrase.maxLevelModifier - Mathf.Abs(adventurerLevel - phrase.idealLevel) * (phrase.maxLevelModifier / 100f);
        }
        return ((phrase.impact * multiplier) + levelMod);
    }

    public void OnFinishEnter()
    {
        controller.GetComponent<Controller>().BeginDialogueSequence();
    }
}

public enum CharacterClass
{
    Warrior,
    Mage,
    Support,
    Rogue
}