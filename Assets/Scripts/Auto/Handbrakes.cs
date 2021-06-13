using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handbrakes : MonoBehaviour
{
    public List<AudioClip> sounds;
    private AudioSource source;
    private CarModel controller;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        controller = GetComponentInParent<CarModel>();
    }

    private void Update()
    {
        if (!controller.photonView.IsMine) return;
        if (controller.Handbracke)
        {
            if (controller._rb.velocity.sqrMagnitude > 8)
            {
                if (!source.isPlaying || source.clip != sounds[Sounds.handbrakes])
                {
                    source.clip = sounds[Sounds.handbrakes];
                    source.Play();
                }
            }
            else
            {
                if (source.volume > 0.1f)
                    source.volume -= Time.deltaTime;
                else
                {
                    source.Stop();
                    source.volume = 1;
                }

            }
        }
        else
        {
            source.Stop();
        }
      
    }

}
