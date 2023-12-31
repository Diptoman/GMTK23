using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
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
    [BoxGroup("Other UI")]
    public GameObject questText;
    [BoxGroup("Other UI")]
    public GameObject dayText;
    [BoxGroup("Other UI")]
    public GameObject dayImage;
    [BoxGroup("Other UI")]
    public GameObject holder;

    [BoxGroup("Timer UI")]
    public GameObject timerText;
    [BoxGroup("Timer UI")]
    public GameObject timerRight;
    [BoxGroup("Timer UI")]
    public GameObject timerLeft;

    [BoxGroup("Main Menu UI")]
    public GameObject mainHeader;

    [BoxGroup("Game Over UI")]
    public GameObject gameOver;
    [BoxGroup("Game Over UI")]
    public GameObject gameOverMainText;
    [BoxGroup("Game Over UI")]
    public GameObject gameOverSideText;

    [BoxGroup("Typing")]
    public float typingSpeed = 0.0001f;
    
    private TextMeshProUGUI dialogueText;
    private QuestLine selectedQuest;
    private int questLinePosition = 0;

    private float currentDayTimer;
    private float maxDayTimer;
    private bool isOutOfTime = false;

    private AudioSource audioSource;
    public AudioClip newDay;
    public AudioClip timer;
    public bool timerBegun = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText = dialogueTextObj.GetComponent<TextMeshProUGUI>();

        //Move dialogue box down
        LeanTween.moveLocalY(dialogueBox, -1000f, 0f);

        //Remove Game Over screen
        LeanTween.alpha(gameOver.GetComponent<RectTransform>(), 0f, 0f);
        LeanTween.scale(gameOver.GetComponent<RectTransform>(), Vector3.zero, 0f);

        //Remove day image
        LeanTween.moveLocalX(dayImage, 1000f, 0f);

        audioSource = GetComponent<AudioSource>();

        holder.SetActive(false);

        //Move menu
        LeanTween.moveLocalY(mainHeader, -1000f, 0f);
        LeanTween.moveLocalY(mainHeader, 170f, 0.75f).setEase(LeanTweenType.easeInOutQuint);
    }

    private void Update()
    {
        if (controllerReference.GetComponent<Controller>().gameBegun)
        {
            //Update timer
            if (currentDayTimer >= 0f)
            {
                currentDayTimer -= Time.deltaTime;
                timerText.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(currentDayTimer).ToString();
                float timerPercentage = (currentDayTimer / maxDayTimer);
                timerRight.GetComponent<RectTransform>().localScale = new Vector3(timerPercentage, 1f, 1f);
                timerLeft.GetComponent<RectTransform>().localScale = new Vector3(timerPercentage, 1f, 1f);

                if (currentDayTimer <= 10f && !timerBegun)
                {
                    audioSource.clip = timer;
                    audioSource.loop = true;
                    audioSource.Play();
                    timerBegun = true;
                }
            }
            else if (!isOutOfTime)
            {
                controllerReference.GetComponent<Controller>().GameOver(EndReason.OutOfTime);
                isOutOfTime = true;
                audioSource.Stop();
                timerBegun = false;
            }
        }
    }

    public void StartGame()
    {
        holder.SetActive(true);
    }

    public void EndMenu()
    {
        LeanTween.moveLocalY(mainHeader, -1000f, 0.75f).setEase(LeanTweenType.easeInOutQuint);
    }

    public void BeginDay(float timer)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = newDay;
        audioSource.loop = false;
        audioSource.Play();

        maxDayTimer = timer;
        currentDayTimer = timer;
        isOutOfTime = false;
        timerBegun = false;
        UpdateTaskText();
        UpdateDayText();
    }

    public void GameOver(EndReason reason)
    {
        //Add Game Over screen
        LeanTween.scale(gameOver.GetComponent<RectTransform>(), Vector3.one, 0f);
        LeanTween.alpha(gameOver.GetComponent<RectTransform>(), 1f, 0.5f);
        string primaryText = "";
        string secondaryText = "";

        //Set text
        switch(reason)
        {
            case EndReason.OutOfTime:
                primaryText = "You couldn't fulfill today's quota!";
                secondaryText += "You've been fired from the job for failing to meet deadlines. Welcome to being an average Joe in a game, bud.";
                break;
            case EndReason.KilledByWarrior:
                primaryText = "You were mauled by a warrior!";
                secondaryText += "Your quest made the warrior angry. They beat you up. You don't have medical insurance to cover the costs, so you die.";
                break;
            case EndReason.KilledByMage:
                primaryText = "You were obliterated by a mage!";
                secondaryText += "Your quest made the mage angry. They used some shoddy magic to give you diarrhea, which accidentally made you explode.";
                break;
            case EndReason.KilledBySupport:
                primaryText = "You were bodyshamed by a support!";
                secondaryText += "Your quest made the support angry. They said some very mean words to you, which made you sad and you quit your NPC job.";
                break;
            case EndReason.KilledByRogue:
                primaryText = "You were stabbed by a rogue!";
                secondaryText += "Your quest made the rogue angry. They backstabbed and frontstabbed and sidestabbed you. You'll probably survive but this job isn't for you.";
                break;
        }

        secondaryText += "\n\nYou managed " + controllerReference.GetComponent<Controller>().currentDay + " day(s) without messing it up.";

        //Add mood numbers
        secondaryText += "\n\n TALLY!\n Ecstatic adventurers: " + controllerReference.GetComponent<Controller>().moodTracker[Mood.Ecstatic];
        secondaryText += "\n Happy adventurers: " + controllerReference.GetComponent<Controller>().moodTracker[Mood.Happy];
        secondaryText += "\n Indifferent adventurers: " + controllerReference.GetComponent<Controller>().moodTracker[Mood.Neutral];
        secondaryText += "\n Confused adventurers: " + controllerReference.GetComponent<Controller>().moodTracker[Mood.Confused];
        secondaryText += "\n Angry adventurer: " + controllerReference.GetComponent<Controller>().moodTracker[Mood.Angry];

        secondaryText += "\n\n\n Enter/Space to restart";

        StartCoroutine(ShowGameOverText(.5f, primaryText, secondaryText));

        //Remove quest text
        holder.SetActive(false);
    }

    public void ResetGame()
    {
        //Move dialogue box down
        LeanTween.moveLocalY(dialogueBox, -1000f, 0f);

        if (buttonReferences.Count > 0)
        {
            foreach(GameObject button in buttonReferences) 
            {
                Destroy(button);
            }
            buttonReferences.Clear();
        }

        //Remove Game Over screen
        LeanTween.alpha(gameOver.GetComponent<RectTransform>(), 0f, 0.5f);
        LeanTween.scale(gameOver.GetComponent<RectTransform>(), Vector3.zero, 0f);
        //Add back texts
        holder.SetActive(true);
        StartCoroutine(ShowGameOverText(0f, "", ""));
    }

    private IEnumerator ShowGameOverText(float delay, string primaryText, string secondaryText)
    {
        yield return new WaitForSeconds(delay);
        gameOverMainText.GetComponent<TextMeshProUGUI>().text = primaryText;
        gameOverSideText.GetComponent<TextMeshProUGUI>().text = secondaryText;
    }

    public void BeginDialogue()
    {
        dialogueText.text = ""; //Reset text
        questLinePosition = 0;
        LeanTween.moveLocalY(dialogueBox, 50f, .75f).setEase(LeanTweenType.easeInOutQuint);

        //Get the sentence structures
        List<QuestLine> questStructures = new List<QuestLine>();
        questStructures = controllerReference.GetComponent<Controller>().questStructures;
        
        //Select a quest
        selectedQuest = questStructures[Random.Range(0, questStructures.Count)];

        ParseQuestLine(.75f);
    }

    public void EndDialogue()
    {
        LeanTween.moveLocalY(dialogueBox, 100f, .5f).setEase(LeanTweenType.easeInOutQuint);
        LeanTween.moveLocalY(dialogueBox, -1000f, .5f).setDelay(2f).setEase(LeanTweenType.easeInOutQuint); //Reset Y
        UpdateTaskText();
    }

    public void ParseQuestLine(float delay)
    {
        if (selectedQuest.phrases.Count >= questLinePosition + 1)
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
            controllerReference.GetComponent<Controller>().DecideQuestResult();
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
            LeanTween.moveLocalY(buttonRef, -210f - currentIndex*80f, 0.75f).setEase(LeanTweenType.easeInOutQuint).setDelay(currentIndex * .2f);
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
            LeanTween.alpha(button.gameObject.GetComponent<RectTransform>(), 0f, .75f).setEase(LeanTweenType.easeInOutQuint).setDelay(currentIndex * .1f);
            button.GetComponent<Button>().interactable = false;
            Destroy(button, 2f);
            currentIndex++;
        }
        buttonReferences.Clear();

        ParseQuestLine(0);
    }

    public void ShowMoodIndicator(Mood mood, int size = 1)
    {
        GameObject moodBox = Instantiate(moodPrefab, transform);
        moodBox.GetComponent<MoodBox>().SetMood(mood, size);
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

    public void UpdateTaskText()
    {
        float totalTasks = controllerReference.GetComponent<Controller>().currentDayTaskNum;
        float completedTasks = controllerReference.GetComponent<Controller>().dayTasksCompleted;
        questText.GetComponent<TextMeshProUGUI>().text = "Tasks completed: " + completedTasks + "/" + totalTasks;
    }

    public void UpdateDayText()
    {
        //Remove day image
        LeanTween.moveLocalX(dayImage, 600f, 0.5f).setEase(LeanTweenType.easeInOutQuint);
        LeanTween.moveLocalX(dayImage, 1000f, 0.5f).setEase(LeanTweenType.easeInOutQuint).setDelay(3f);
        int day = controllerReference.GetComponent<Controller>().currentDay;
        dayText.GetComponent<TextMeshProUGUI>().text = "Day: " + day;
    }
}
