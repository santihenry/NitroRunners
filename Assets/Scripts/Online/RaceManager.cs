﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class RaceManager : MonoBehaviourPun
{

    private bool _startRace;
    float currentTime;
    public TMP_Text timerTxt;
    public int timeToStartRace;

    private bool _calentamiento;


    public Material ligthOff, ligthGreen, ligthYellow, ligthRed;
    public GameObject droneSemaforo;
    public GameObject droneSemaforoV;
    public GameObject droneSemaforoR;
    public GameObject droneSemaforoA;
    float delay;

    public static RaceManager Instance { get; set; }

    public Vector3 pos;
    public Vector3 offset;
    Vector3 posOffset;
    bool fin;

    DroneSemaforo drone;
    bool r, a, v;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        StartRace = false;

    }


    private void Start()
    {
        if (timerTxt != null)
            timerTxt.gameObject.SetActive(true);
        currentTime = 0;
        StartRace = false;

        droneSemaforo.SetActive(true);
        droneSemaforoR.SetActive(true);
        droneSemaforoA.SetActive(false);
        droneSemaforoV.SetActive(false);

        drone = droneSemaforo.GetComponent<DroneSemaforo>();
    }

    private void Update()
    {

        if(pos == Vector3.zero)
        {
            pos = FindObjectOfType<CamerMovement>().Car.transform.position;
            posOffset = pos + offset;
            droneSemaforo.transform.position = pos + offset;
        }

 

        if(PhotonNetwork.CurrentRoom.PlayerCount == FindObjectsOfType<CarModel>().Length)
        {
            currentTime += Time.deltaTime;
            Calentamiento = false;
            if(droneSemaforo != null)
            {
                droneSemaforo.SetActive(true);
                var tempPos = posOffset;
                tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 1) * .4f;
                if(!fin)
                    droneSemaforo.transform.position = tempPos;
            }
        }
        else
        {
            Calentamiento = true;
            if (droneSemaforo != null)
                droneSemaforo.SetActive(false);
        }


      

        if (timeToStartRace - currentTime < 0)
        {
            StartRace = true;
            timerTxt.text = timerTxt.text = "";
            timerTxt.gameObject.SetActive(false);

            if (droneSemaforo.activeSelf)
            {
                droneSemaforoR.SetActive(false);
                droneSemaforoA.SetActive(false);
                droneSemaforoV.SetActive(true);
            }

            if (currentTime > timeToStartRace + .5f)
            {
                fin = true;
                droneSemaforo.transform.position += Vector3.up * 12 * Time.deltaTime;
                if (!drone.source.isPlaying && !v)
                {
                    drone.source.clip = drone.sounds[1];
                    drone.source.Play();
                    v = true;
                }
            }
            if (currentTime > timeToStartRace + 2)
            {
                droneSemaforo.SetActive(false);
                Destroy(droneSemaforo);
            }
        }
        else
        {
          
            //timerTxt.text = (timeToStartRace - currentTime).ToString("N0");
            if (timeToStartRace - currentTime > 2)
            {
                droneSemaforoR.SetActive(true);
                droneSemaforoA.SetActive(false);
                droneSemaforoV.SetActive(false);
                if (!drone.source.isPlaying && !r)
                {
                    drone.source.clip = drone.sounds[0];
                    drone.source.Play();
                    r = true;
                }
            }
            else if(timeToStartRace - currentTime < 2 && timeToStartRace - currentTime > 1)
            {
                droneSemaforoR.SetActive(false);
                droneSemaforoA.SetActive(true);
                droneSemaforoV.SetActive(false);
                if (!drone.source.isPlaying  && !a)
                {
                    drone.source.clip = drone.sounds[0];
                    drone.source.Play();
                    a = true;
                }
            }
            else if (timeToStartRace - currentTime < 1 && timeToStartRace - currentTime == 0 ) 
            {
                droneSemaforoR.SetActive(false);
                droneSemaforoA.SetActive(false);
                droneSemaforoV.SetActive(true);               
            }

        }
    }
    


    public bool StartRace
    {
        get
        {
            return _startRace;
        }
        set
        {
            _startRace = value;
        }
    }

    public bool Calentamiento
    {
        get
        {
            return _calentamiento;
        }
        set
        {
            _calentamiento = value;
        }
    }


}
