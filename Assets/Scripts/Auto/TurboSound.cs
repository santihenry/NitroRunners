using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboSound : MonoBehaviour
{
    private CarModel controller;
    public List<AudioClip> sounds;
    private AudioSource source;

  

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        controller = GetComponentInParent<CarModel>();
    }

    private void Update()
    {
        if (!controller.photonView.IsMine) return;
        Turbo();
    }

    private void Turbo()
    {
        if (controller.Bosting == true && !source.isPlaying || source.clip != sounds[Sounds.turbo] && controller.Bosting == true)
        {
            source.clip = sounds[Sounds.turbo];
            source.Play();
        }
        else if (controller.Bosting == false)
        {
            source.Stop();
        }
    }

}
