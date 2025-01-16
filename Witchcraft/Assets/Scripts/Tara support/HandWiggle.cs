using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandWiggle : MonoBehaviour
{
    public RectTransform hand;
    public float speed = 1f;

    void Update()
    {
        float y = Mathf.PingPong(Time.time * speed, 1) * 341 - 538;
        hand.anchoredPosition = new Vector2(-864, y);
    }
}
