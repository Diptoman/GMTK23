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
        return (phrase.impact * multiplier);
    }

    public void OnFinishEnter()
    {
        controller.GetComponent<Controller>().BeginDialogueSequence();
    }

    public void OnFinishExit()
    {
        controller.GetComponent<Controller>().EndRound();
        Destroy(this.gameObject);
    }

    public void OnFinishAttack()
    {
        EndReason reason = EndReason.OutOfTime;
        Destroy(this.gameObject, .5f);
        switch (characterClass)
        {
            case CharacterClass.Warrior: reason = EndReason.KilledByWarrior; break;
            case CharacterClass.Mage: reason = EndReason.KilledByMage; break;
            case CharacterClass.Support: reason = EndReason.KilledBySupport; break;
            case CharacterClass.Rogue: reason = EndReason.KilledByRogue; break;
        }
        controller.GetComponent<Controller>().GameOver(reason);
    }

    public void FuggOff()
    {
        GetComponent<Animator>().SetTrigger("FuggOff");
    }

    public void BitchSlap()
    {
        GetComponent<Animator>().SetTrigger("BitchSlap");
    }
}

public enum CharacterClass
{
    Warrior,
    Mage,
    Support,
    Rogue
}