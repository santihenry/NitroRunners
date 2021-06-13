using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public List<AudioClip> sounds;
    private AudioSource source;
    float respawnTime = 5;
    float currentTime;
    bool off;


    Roulete ruleta;


    CarModel car;



    private void Awake()
    {
        source = GetComponent<AudioSource>();
        ruleta = FindObjectOfType<Roulete>();
    }

    void Update()
    {
        if (GameManager.Instance.finishRace) Destroy(gameObject, 0.5f);

        transform.Rotate(Vector3.up, 1f);
        if (off)
        {
            currentTime += Time.deltaTime;
            if (respawnTime < currentTime)
            {
                GetComponent<Collider>().enabled = true;
                GetComponent<Renderer>().enabled = true;
                off = false;
                currentTime = 0;
            }
        }
    }


    public void TakeItem()
    {
        ruleta.TakeItem();      
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarModel>())
        {
            car = other.GetComponent<CarModel>();
            GetComponent<Collider>().enabled = false;
            source.clip = sounds[Sounds.pickItem];
            source.Play();
            GetComponent<Renderer>().enabled = false;//Destroy(gameObject, 0.5f);
            off = true;           
        }
    }
}