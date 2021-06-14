using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Pun;
using System.Linq;
using UnityEngine.UI;



public class Positions : MonoBehaviourPun
{
    public TMP_Text debugPositionsTxt;
    public List<CarModel> posList = new List<CarModel>();
    public Dictionary<CarModel, int> dicPosition = new Dictionary<CarModel, int>();
    public List<CarModel> cars = new List<CarModel>();

    public GameObject debug;

    public GameObject fbxTrackMinimap;


    public Image primero;
    public Image segundo;
    public Image tercero;
    
    private void Start()
    {
        if(debug != null)
            debug.SetActive(false);

        fbxTrackMinimap.SetActive(true);

    }

    private void Update()
    {
        UpdateList();

        foreach (var item in FindObjectsOfType<CarModel>())
        {          
            if (!cars.Contains(item))
                cars.Add(item);
        }

        if (Input.GetKeyDown(KeyCode.I) &&  debug != null)
        {
            debug.SetActive(!debug.activeSelf);
        }

    }


    void UpdateList()
    {
        foreach (CarModel car in cars)
        {
            int distToMeta = 0;
            distToMeta = car.currentWay == car._waypointsList.Count-1 || car.currentWay == 0 ?  (int)Vector3.Distance(car.transform.position, car._waypointsList[0].position) : 10000;

            if (!dicPosition.ContainsKey(car))
            {
                dicPosition.Add(car, distToMeta);
                posList.Add(car);
            }
            else
                dicPosition[car] = distToMeta;

            car.DistToMeta = distToMeta;
            var ordList = SortCars();
            posList = new List<CarModel>();
            posList.AddRange(ordList);

            for (int i = 0; i < posList.Count; i++)
            {
                posList[i].Pos = i+1;
            }
        }

        var posTxt = "\n";
        foreach (var car in posList)
        {           
            posTxt += $"{car.nickName} | Lap : {car.Lap} Waypoint : {car.currentWay} | Position:{car.Pos} | Dist:{car.DistToMeta} \n";            
        }     
        debugPositionsTxt.text = posTxt;



        if (posList.Any() && !GameManager.Instance.finishRace) 
        {
            if (posList.Count > 0)
            {
                primero.gameObject.SetActive(true);
                primero.sprite = posList[0].statsData.pjImg;
                primero.GetComponentInChildren<Text>().text = $"<size=20>1°</size> <size=15>{posList[0].nickName}</size>";
            }
            else
                primero.gameObject.SetActive(false);

            if (posList.Count > 1)
            {
                segundo.gameObject.SetActive(true);
                segundo.sprite = posList[1].statsData.pjImg;
                segundo.GetComponentInChildren<Text>().text = $"<size=20>2°</size> <size=15>{posList[1].nickName}</size>";
            }
            else
                segundo.gameObject.SetActive(false);

            if (posList.Count > 2)
            {
                tercero.gameObject.SetActive(true);
                tercero.sprite = posList[2].statsData.pjImg;
                tercero.GetComponentInChildren<Text>().text = $"<size=20>3°</size> <size=15>{posList[2].nickName}</size>";
            }
            else
                tercero.gameObject.SetActive(false);
        }else if (GameManager.Instance.finishRace)
        {
            primero.gameObject.SetActive(false);
            segundo.gameObject.SetActive(false);
            tercero.gameObject.SetActive(false);
        }
    }


    private IEnumerable<CarModel> SortCars()
    { 
        return posList.OrderBy(car => car.DistToMeta).OrderByDescending(car => car.currentWay).OrderByDescending(car => car.Lap);   
    }

}
