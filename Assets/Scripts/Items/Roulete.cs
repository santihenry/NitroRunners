using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Roulete : MonoBehaviour
{

    public Image rouleteImg;
    public Image frameRouleteImg;
    public List<Sprite> sprites = new List<Sprite>();
    bool starRoulete;


    Dictionary<int, Sprite> itemsDic = new Dictionary<int, Sprite>();

    [Range(0, 1)]
    public float delay;
    float currentTimeDelay;
    float currentTimeChangeDelay;
    float currentTimeRoulete;
    public float rouleteTime;
    float startDelay;


    public bool finish;
    public int itemWin;

    AudioSource source;
    public List<AudioClip> sounds;

    public bool CanDrop
    {
        get
        {
            return starRoulete ? false : true;
        }
    }


    private void Start()
    {
        startDelay = delay;
        rouleteImg.gameObject.SetActive(false);
        frameRouleteImg.gameObject.SetActive(false);
        

        for (int i = 0; i < sprites.Count; i++)
        {
            itemsDic.Add(i, sprites[i]);
        }

        source = rouleteImg.GetComponent<AudioSource>();
    }

    void Update()
    {
        currentTimeDelay += Time.deltaTime;
        currentTimeChangeDelay += Time.deltaTime;
        currentTimeRoulete += Time.deltaTime;


        RouleteWheel();


        if (!finish && currentTimeRoulete >= rouleteTime || Input.GetKeyUp(KeyCode.LeftShift))
        {
            source.Stop();
            currentTimeDelay = 0;
            currentTimeChangeDelay = 0;
            currentTimeRoulete = 0;
            starRoulete = false;
            finish = true;
        }
    }

    public void RouleteWheel()
    {
        if (starRoulete)
        {
            delay = rouleteTime - currentTimeChangeDelay < rouleteTime / 2.2f ? startDelay + .2f : startDelay;


            if (!source.isPlaying && source.clip != sounds[Sounds.rouletteSpin])
            {
                source.clip = sounds[Sounds.rouletteSpin];
                source.Play();
            }

            if (delay - currentTimeDelay <= 0)
            {
                itemWin = Random.Range(0, sprites.Count);
                rouleteImg.sprite = itemsDic[itemWin];
                currentTimeDelay = 0;
            }
        }
    }



    public void RouleteNormal()
    {
        if (starRoulete)
        {
            delay = rouleteTime - currentTimeChangeDelay < rouleteTime / 2.2f ? startDelay + .2f : startDelay;


            if (!source.isPlaying && source.clip != sounds[Sounds.rouletteSpin])
            {
                source.clip = sounds[Sounds.rouletteSpin];
                source.Play();
            }

            if (delay - currentTimeDelay <= 0)
            {
                itemWin = Random.Range(0, sprites.Count);
                rouleteImg.sprite = itemsDic[itemWin];
                currentTimeDelay = 0;
            }
        }
    }


    public void TakeItem()
    {
        rouleteImg.gameObject.SetActive(true);
        frameRouleteImg.gameObject.SetActive(true);
        starRoulete = true;
        currentTimeDelay = 0;
        currentTimeChangeDelay = 0;
        currentTimeRoulete = 0;
        finish = false;
    }


}
