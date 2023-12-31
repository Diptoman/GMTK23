using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class MoodImage : MonoBehaviour
{
    private Image image;

    public Sprite[] moodSprites;

    void Awake()
    {
        image = this.GetComponent<Image>();
    }

    public void SetMood(Mood mood)
    {
        image.sprite = moodSprites[(int)mood];
    }
}
