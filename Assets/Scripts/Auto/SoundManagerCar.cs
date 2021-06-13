using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerCar : MonoBehaviour
{
    CarModel controller;

    public List<AudioClip> sounds;
    private AudioSource source;
    float _startingPitch=0.1f;
    float _maxPitch=1f;
    float _minVolume=0.4f;
    float _maxVolume= 1.3f;
    

    private void Awake()
    {
        controller = GetComponent<CarModel>();
        source = GetComponent<AudioSource>();
        source.pitch = _startingPitch;
    }

    private void Update()
    {
        if (!controller.photonView.IsMine) return;
        SoundAcelerate();
        source.pitch = Mathf.Clamp(controller._rb.velocity.sqrMagnitude / 2  / controller.MaxSpeed,_startingPitch,_maxPitch);
        source.volume = Mathf.Clamp(controller._rb.velocity.sqrMagnitude / 2 / controller.MaxSpeed, _minVolume, _maxVolume);     
    }

    private void SoundAcelerate()
    {
        if (controller._rb.velocity.sqrMagnitude > 0)
        {
            if (!source.isPlaying && controller.Bosting == false || source.clip != sounds[Sounds.engine] && controller.Bosting == false)
            {
                source.clip = sounds[Sounds.engine];
                source.Play();                
            }
        }
        else
        {
            source.Stop();
        }
    }

}