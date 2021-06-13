using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWall : MonoBehaviour
{
    CarModel controller;
    public List<AudioClip> sounds;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        source.clip = sounds[Sounds.forceField];
        source.Play();
    }
}
