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
        track = 0;//track = Random.Range(0, sounds.Count);
        source.clip = sounds[track];
        source.Play();
    }

    void Update()
    {
        Fijo();
    }


    public void Fijo()
    {
        if (!source.isPlaying)
        {
            track++;
            if (track >= sounds.Count) track = 0;
            source.clip = sounds[track];
            source.Play();
        }
    }

    public void Aleatorio()
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
