using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [BoxGroup("References")]
    public GameObject controllerReference;

    [BoxGroup("Dialogue UI")]
    public GameObject dialogueBox;
    [BoxGroup("Dialogue UI")]
    public GameObject dialogueTextObj;
    [BoxGroup("Dialogue UI")]
    public List<GameObject> buttonReferences = new List<GameObject>();
    [BoxGroup("Dialogue UI")]
    public GameObject buttonPrefab;

    [BoxGroup("Other UI")]
    public GameObject moodPrefab;

    [BoxGroup("Typing")]
    public float typingSpeed = 0.04f;
    
    private TextMeshProUGUI dialogueText;
    private QuestLine selectedQuest;
    private int questLinePosition = 0;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText = dialogueTextObj.GetComponent<TextMeshProUGUI>();

        //Move dialogue box down
        LeanTween.moveLocalY(dialogueBox, -1000f, 0f);
        
    }

    public void BeginDialogue()
    {
        LeanTween.moveLocalY(dialogueBox, 50f, .75f).setEase(LeanTweenType.easeInOutQuint);

        //Get the sentence structures
        List<QuestLine> questStructures = new List<QuestLine>();
        questStructures = controllerReference.GetComponent<Controller>().questStructures;
        
        //Select a quest
        selectedQuest = questStructures[Random.Range(0, questStructures.Count - 1)];

        ParseQuestLine(.75f);
    }

    public void ParseQuestLine(float delay)
    {
        if (selectedQuest.phrases.Count > questLinePosition + 1)
        {
            if (!selectedQuest.phrases[questLinePosition].useExistingPhraseHere)
            {
                StartCoroutine(ContinueTyping(delay, selectedQuest.phrases[questLinePosition].phrase, 0));
            }
            else
            {
                CreateOptions();
            }
        }
        else
        {
            DecideQuestResult();
        }

        questLinePosition++;
    }

    public void CreateOptions()
    {
        //Select random words from specified group
        List<PhraseObject> words = controllerReference.GetComponent<Controller>().PickOptions(selectedQuest.phrases[questLinePosition].wordClassification, 3);
        int currentIndex = 0;

        //Create buttons
        for(int i=0; i < words.Count; i++) 
        {
            GameObject option = Instantiate(buttonPrefab, dialogueBox.transform);
            buttonReferences.Add(option);
            LeanTween.moveLocalY(option, -1000f, 0f);

            //Bind
            option.GetComponent<Button>().onClick.AddListener(delegate { OptionSelected(option); });
        }

        //Move up buttons
        foreach (GameObject buttonRef in buttonReferences)
        {
            LeanTween.moveLocalY(buttonRef, -200f - currentIndex*70f, 0.75f).setEase(LeanTweenType.easeInOutQuint).setDelay(currentIndex * .2f);
            buttonRef.GetComponent<OptionButton>().Initialize(words[currentIndex], selectedQuest.phrases[questLinePosition].wordClassification);
            currentIndex++;
        }
    }

    public void OptionSelected(GameObject buttonRef)
    {
        string text = buttonRef.GetComponentInChildren<TextMeshProUGUI>().text;
        int currentIndex = 0;
        dialogueText.text += text;

        //Parse the phrase
        controllerReference.GetComponent<Controller>().ParseCurrentPhrase(buttonRef.GetComponent<OptionButton>().GetAssociatedPhrase());

        //Move down buttons
        foreach (GameObject button in buttonReferences)
        {
            LeanTween.moveLocalX(button, 5000f, .75f).setEase(LeanTweenType.easeInOutQuint).setDelay(currentIndex * .1f);
            Destroy(button, 2f);
            currentIndex++;
        }
        buttonReferences.Clear();

        ParseQuestLine(0);
    }

    public void DecideQuestResult()
    {

    }

    public void ShowPhraseMoodIndicator(Mood mood)
    {
        GameObject moodBox = Instantiate(moodPrefab, transform);
        moodBox.GetComponent<MoodBox>().SetMood(mood);
    }

    private IEnumerator ContinueTyping(float delay, string text, int positionInQuest)
    {
        yield return new WaitForSeconds(delay);
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        ParseQuestLine(0f);
    }
}
