using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodBox : MonoBehaviour
{
    public MoodImage moodImage;
    private Mood currentMood;

    public void SetMood(Mood mood, int size = 1)
    {
        currentMood = mood;
        moodImage.SetMood(mood);

        if (size == 2)
        {
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 300f);
            this.moodImage.GetComponent<RectTransform>().sizeDelta = new Vector2(192, 192);
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
