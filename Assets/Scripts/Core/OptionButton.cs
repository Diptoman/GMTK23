using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    private PhraseObject phraseObj;

    public void Initialize(PhraseObject phrase, WordClassification wordClass)
    {
        //Pick alternate if needed
        bool useAlternate = false;
        switch (wordClass)
        {
            case WordClassification.Things: useAlternate = true; break;
            case WordClassification.VerbPresent: useAlternate = true; break;
        }

        GetComponentInChildren<TextMeshProUGUI>().text = useAlternate == false ? phrase.word : phrase.alternateWord;
        phraseObj = phrase;

    }

    public PhraseObject GetAssociatedPhrase()
    {
        return phraseObj;
    }
}
