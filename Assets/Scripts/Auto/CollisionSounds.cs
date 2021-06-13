using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
    private CarModel controller;
    public List<AudioClip> sounds;
    private AudioSource source;



    private void Awake()
    {
        source = GetComponent<AudioSource>();
        controller = GetComponentInParent<CarModel>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CarModel>() || collision.gameObject.layer == 9)
        {
            if (!source.isPlaying || source.clip != sounds[Sounds.hitCar])
            {
                source.clip = sounds[Sounds.hitCar];
                source.Play();
            }
        }
        if (collision.gameObject.GetComponent<ItemWall>())
        {
            if (!source.isPlaying || source.clip != sounds[Sounds.hitWall])
            {
                source.clip = sounds[Sounds.hitWall];
                source.Play();
            }
        }
    }


}
