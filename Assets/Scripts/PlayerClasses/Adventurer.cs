using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    public int adventurerLevel = 1;
    public CharacterClass characterClass;
    public GameObject controller; //So much tightly coupled poor design for time's sake ;_;

    private AudioSource audioSource;
    public AudioClip enter;
    public AudioClip exit;
    public AudioClip murder;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = enter;
        audioSource.loop = false;
        audioSource.volume = 1f;
        audioSource.Play();
    }

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
        audioSource.volume = 1f;
        audioSource.PlayOneShot(exit);
    }

    public void BitchSlap()
    {
        GetComponent<Animator>().SetTrigger("BitchSlap");
        audioSource.clip = murder;
        audioSource.loop = false;
        audioSource.volume = .75f;
        audioSource.Play();
    }
}

public enum CharacterClass
{
    Warrior,
    Mage,
    Support,
    Rogue
}