using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodBox : MonoBehaviour
{
    public GameObject moodImage;
    private Mood currentMood;

    public void SetMood(Mood mood, int size = 1)
    {
        currentMood = mood;
        Image moodImg = moodImage.GetComponent<Image>();
        switch(mood)
        {
            case Mood.Angry: moodImg.color = Color.red; break;
            case Mood.Confused: moodImg.color = Color.yellow; break;
            case Mood.Neutral: moodImg.color = Color.blue; break;
            case Mood.Happy: moodImg.color = Color.white; break;
            case Mood.Ecstatic: moodImg.color = Color.green; break;
        }

        if (size == 2)
        {
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 300f);
            LeanTween.moveLocalY(this.gameObject, -1000f, 0f);
            LeanTween.moveLocalX(this.gameObject, 550f, 0f);
            LeanTween.moveLocalY(this.gameObject, -150f, 0.75f).setEase(LeanTweenType.easeInOutQuint);
            LeanTween.alpha(this.gameObject.GetComponent<RectTransform>(), 0f, 1f).setEase(LeanTweenType.easeInOutQuint).setDelay(1.5f);
            Destroy(this.gameObject, 3.5f);
        }
        else
        {
            LeanTween.moveLocalY(this.gameObject, -1000f, 0f);
            LeanTween.moveLocalX(this.gameObject, 550f, 0f);
            LeanTween.moveLocalY(this.gameObject, 150f, 0.75f).setEase(LeanTweenType.easeInOutQuint);
            LeanTween.alpha(this.gameObject.GetComponent<RectTransform>(), 0f, 1f).setEase(LeanTweenType.easeInOutQuint).setDelay(1.5f);
            Destroy(this.gameObject, 3.5f);
        }
    }
}
