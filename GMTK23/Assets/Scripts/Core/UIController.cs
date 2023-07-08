using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject dialogueBox;
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
        StartCoroutine(ContinueTyping(.75f, "LADEEDA dfgsf sdf dfg dfg dsag dsfhg dsfgh df"));
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
