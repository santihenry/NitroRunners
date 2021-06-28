using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Roulete : MonoBehaviour
{

    public Image rouleteImg;
    public Image frameRouleteImg;
    public List<Sprite> sprites = new List<Sprite>();
    bool starRoulete;


    Dictionary<int, Sprite> itemsDic = new Dictionary<int, Sprite>();
    Dictionary<int, int> itemsPesosDic = new Dictionary<int, int>();

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

    CarModel _car;

    public List<int> pesos = new List<int>();

    public TMP_Text debugTxt;

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
            itemsPesosDic.Add(i, 20);
            pesos.Add(20);
        }



        source = rouleteImg.GetComponent<AudioSource>();

        debugTxt.gameObject.SetActive(false);

    }

    int c = 0;

    void Update()
    {
        if(_car == null && c == 0)
        {
            foreach (var item in FindObjectsOfType<CarModel>())
            {
                if (item.nickName == PhotonNetwork.LocalPlayer.NickName)
                {
                    _car = item;
                    c++;
                }
            }
        }

        if(_car != null)
            debugTxt.text = $"Auto :  {_car.name} \n " + $"{_car.nickName} \n { PhotonNetwork.NickName }";

        if (Input.GetKeyDown(KeyCode.P)) 
            debugTxt.gameObject.SetActive(!debugTxt.gameObject.activeSelf);


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

    void Pesos()
    {

        if(_car != null)
        {
            if (_car.ultimo)
            {
                itemsPesosDic[0] = 50; //rocket
                itemsPesosDic[1] = 150; //wall
                itemsPesosDic[2] = 100; //landeMine
                itemsPesosDic[3] = 5; //shield
                itemsPesosDic[4] = 20; //zap
                itemsPesosDic[5] = 30; //scan
                itemsPesosDic[6] = 250; //drone
            }
            else if(_car.Pos <= 2)
            {
                itemsPesosDic[0] = 100; //rocket
                itemsPesosDic[1] = 1; //wall
                itemsPesosDic[2] = 1; //landeMine
                itemsPesosDic[3] = 20; //shield
                itemsPesosDic[4] = 30; //zap
                itemsPesosDic[5] = 30; //scan
                itemsPesosDic[6] = 1; //drone
            }
            else if(_car.Pos <= 4)
            {
                itemsPesosDic[0] = 100; //rocket
                itemsPesosDic[1] = 1; //wall
                itemsPesosDic[2] = 1; //landeMine
                itemsPesosDic[3] = 10; //shield
                itemsPesosDic[4] = 80; //zap
                itemsPesosDic[5] = 30; //scan
                itemsPesosDic[6] = 5; //drone
            }
            else if (_car.Pos > 4)
            {
                itemsPesosDic[0] = 70; //rocket
                itemsPesosDic[1] = 100; //wall
                itemsPesosDic[2] = 80; //landeMine
                itemsPesosDic[3] = 25; //shield
                itemsPesosDic[4] = 50; //zap
                itemsPesosDic[5] = 30; //scan
                itemsPesosDic[6] = 200; //drone
            }

            for (int i = 0; i < itemsPesosDic.Count; i++)
            {
                pesos[i] = itemsPesosDic[i];
            }
        }
        else
        {
            itemsPesosDic[0] = 100; //rocket
            itemsPesosDic[1] = 1; //wall
            itemsPesosDic[2] = 1; //landeMine
            itemsPesosDic[3] = 10; //shield
            itemsPesosDic[4] = 80; //zap
            itemsPesosDic[5] = 30; //scan
            itemsPesosDic[6] = 5; //drone
        }
    }

    public void RouleteWheel()
    {

        Pesos();

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
                itemWin = Execute(itemsPesosDic);//Random.Range(0, sprites.Count);
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


    public int Execute(Dictionary<int, int> actions)
    {
        var maxWeight = 0;
        foreach (var item in actions)
        {
            maxWeight += item.Value;
        }
        float random = Random.Range(0, maxWeight);
        foreach (var currAction in actions)
        {
            random -= currAction.Value;
            if (random < 0)
            {
                return currAction.Key;
            }
        }
        return 0;
    }


}
