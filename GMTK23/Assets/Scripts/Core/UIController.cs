using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [BoxGroup("References")]
    public GameObject controllerReference;

    [BoxGroup("Dialogue UI")]
    public GameObject dialogueBox;
    [BoxGroup("Dialogue UI")]
    public GameObject dialogueTextObj;

    [BoxGroup("Typing")]
    public float typingSpeed = 0.04f;
    
    private TextMeshProUGUI dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText = dialogueTextObj.GetComponent<TextMeshProUGUI>();

        //Move dialogue box down
        LeanTween.moveLocalY(dialogueBox, -500f, 0f);
        BeginDialogue();
        
    }

    public void BeginDialogue()
    {
        LeanTween.moveLocalY(dialogueBox, 50f, .75f).setEase(LeanTweenType.easeInOutQuint);

        //Get the sentence structures
        List<QuestLine> questStructures = new List<QuestLine>();
        questStructures = controllerReference.GetComponent<Controller>().questStructures;
        
        //Select a quest
        QuestLine selectedQuest = questStructures[Random.Range(0, questStructures.Count - 1)];

        //Start writing
        if (!selectedQuest.phrases[0].useExistingPhraseHere)
        {
            StartCoroutine(ContinueTyping(.75f, selectedQuest.phrases[0].phrase));
        }
    }

    private IEnumerator ContinueTyping(float delay, string text)
    {
        yield return new WaitForSeconds(delay);
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
