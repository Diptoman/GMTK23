using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodBox : MonoBehaviour
{
    public GameObject moodImage;
    private Mood currentMood;

    public void SetMood(Mood mood)
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

        LeanTween.moveLocalY(this.gameObject, -1000f, 0f);
        LeanTween.moveLocalX(this.gameObject, 550f, 0f);
        LeanTween.moveLocalY(this.gameObject, 150f, 0.75f).setEase(LeanTweenType.easeInOutQuint);
        LeanTween.moveLocalY(this.gameObject, 1000f, 0.75f).setEase(LeanTweenType.easeInOutQuint).setDelay(2.75f);
        Destroy(this.gameObject, 3.5f);
    }
}
