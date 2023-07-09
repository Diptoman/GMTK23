using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMovement : MonoBehaviour
{
    public float speed = 1f;
    public float amount = 0.025f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        var offset = amount * Mathf.Sin(Time.time * speed);

        var pos = transform.localPosition;
        pos.y += offset;

        transform.localPosition = pos;
    }
}
