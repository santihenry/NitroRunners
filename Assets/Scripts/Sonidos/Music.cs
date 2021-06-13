using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public List<AudioClip> sounds;
    private AudioSource source;
    int track = 0;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        track = Random.Range(0, sounds.Count);
        source.clip = sounds[track];
        source.Play();
    }

    void Update()
    {
        if (!source.isPlaying)
        {
            var newTrack = Random.Range(0, sounds.Count);
            while (track == newTrack)
            {
                newTrack = Random.Range(0, sounds.Count);
            }
            track = newTrack;
            source.clip = sounds[track];
            source.Play();
        }
    }

}
