using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSounds : MonoBehaviour
{
    public List<AudioClip> audioclips;
    public AudioClip currentClip;
    public AudioSource source;

    float minWait = 3f;
    float maxWait = 7f;
    float waitTime = -1f;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!source.isPlaying)
        {
            if (waitTime < 0f)
            {
                currentClip = audioclips[Random.Range(0, audioclips.Count)];
                source.clip = currentClip;
                source.volume = 0.07f;
                source.Play();
                waitTime = Random.Range(minWait, maxWait);
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
