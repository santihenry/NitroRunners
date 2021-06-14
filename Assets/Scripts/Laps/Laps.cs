using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class Laps : MonoBehaviourPun
{
    public int lap;
    public TMP_Text lapsTxt;
    bool checkLap, check1, check2, check3, check4, check5, check6, check7, check8;
    public List<GameObject> checksPoints = new List<GameObject>();
    public TMP_Text clock;
    public float currentTime;
    public int m, s, h;
    public int seg, min, hs;
    public int milis;
    public List<List<int>> saveTimes = new List<List<int>>();
    CarModel _carModel;


    float lasTimeCurrentTime;

    private void Awake()
    {
        if (!photonView.IsMine) return;
        lapsTxt = GameObject.Find("LapsTxt").gameObject.GetComponent<TMP_Text>();
        clock = GameObject.Find("TimerTxt").gameObject.GetComponent<TMP_Text>();
    }

    void Start()
    {
        if (!photonView.IsMine) return;
        lap = 1;
        TakeCheckPoints();
        _carModel = GetComponent<CarModel>();
    }
        
    void Update()
    {
       
        if (!photonView.IsMine) return;
        if (GameManager.Instance.finishRace) return;
        

        if (lap == (int)PhotonNetwork.CurrentRoom.CustomProperties["Laps"] && (int)PhotonNetwork.CurrentRoom.CustomProperties["Laps"] > 1)
        {
            lasTimeCurrentTime += Time.deltaTime;
            var active = lasTimeCurrentTime < 2 ? true : false;
            GameManager.Instance.LastLap.SetActive(active);
        }


        if (lapsTxt != null)
        {
            if (lap > (int)PhotonNetwork.CurrentRoom.CustomProperties["Laps"])
                lapsTxt.text = "";
            else
                lapsTxt.text = $"<size=15>LAP  </size><size=25>{lap}</size><size=15> / {PhotonNetwork.CurrentRoom.CustomProperties["Laps"]}</size>";
        }

        if (checkLap)
        {
            lap++;
            checkLap = false;
            check1 = false;
            check2 = false;
            check3 = false;
            check4 = false;
            check5 = false;
            check6 = false;
            check7 = false;
            check8 = false;
        }
        if (RaceManager.Instance.StartRace)
            photonView.RPC("Clock", RpcTarget.All);

        SaveTimes();
        UpdateDistance();
    }


    void UpdateDistance()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(_carModel.photonView.ViewID))
        {
            var hash = PhotonNetwork.CurrentRoom.CustomProperties;
            var dist = Dist();

            hash[_carModel.photonView.ViewID] = dist;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }
    }

    private void TakeCheckPoints()
    {
        foreach (var checksPoint in FindObjectsOfType<CheckPoint>())
        {
            checksPoints.Add(checksPoint.gameObject);
        }
        var x = Order();
        checksPoints = new List<GameObject>();
        checksPoints.AddRange(x);
    }

    private IEnumerable<GameObject> Order()
    {
        return checksPoints.OrderBy(n => n.GetComponent<CheckPoint>().id);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CheckPoint>())
        {
            var checkPoint = other.GetComponent<CheckPoint>();
            if (checkPoint.isStartLine && check1 && check2 && check3 && check4 && check5 && check6 && check7 && check8) checkLap = true;
            switch (checkPoint.id)
            {
                case 1:
                    if (!check1 && !check2 && !check3 && !check4 && !check5 && !check6 && !check7 && !check8)
                        check1 = true;
                    break;
                case 2:
                    if (check1 && !check2 && !check3 && !check4 && !check5 && !check6 && !check7 && !check8)
                        check2 = true;
                    break;
                case 3:
                    if (check1 && check2 && !check3 && !check4 && !check5 && !check6 && !check7 && !check8)
                        check3 = true;
                    break;
                case 4:
                    if (check1 && check2 && check3 && !check4 && !check5 && !check6 && !check7 && !check8)
                        check4 = true;
                    break;
                case 5:
                    if (check1 && check2 && check3 && check4 && !check5 && !check6 && !check7 && !check8)
                        check5 = true;
                    break;
                case 6:
                    if (check1 && check2 && check3 && check4 && check5 && !check6 && !check7 && !check8)
                        check6 = true;
                    break;
                case 7:
                    if (check1 && check2 && check3 && check4 && check5 && check6 && !check7 && !check8)
                        check7 = true;
                    break;
                case 8:
                    if (check1 && check2 && check3 && check4 && check5 && check6 && check7 && !check8)
                        check8 = true;
                    break;
            }
        }
    }


    [PunRPC]
    private void Clock()
    {
        currentTime += Time.deltaTime;

        s = (int)currentTime;

        if (currentTime >= 60)
        {
            s = 0;
            m += 1;
            currentTime = 0;
        }
        if (m >= 60)
        {
            m = 0;
            h += 1;
        }
        if (h >= 24)
            h = 0;

        if (clock == null) return;
        if (h >= 10 && m >= 10 && s >= 10)
            clock.text = h + ":" + m + ":" + s;
        else if (h < 10 && m >= 10 && s >= 10)
            clock.text = h / 10 + h + ":" + m + ":" + s;
        else if (h < 10 && m < 10 && s >= 10)
            clock.text = h / 10 + h + ":" + m / 10 + m + ":" + s;
        else if (h < 10 && m < 10 && s < 10)
            clock.text = h / 10 + h + ":" + m / 10 + m + ":" + s / 10 + s;
    }
    private void SaveTimes()
    {
        if (lap == 2)
        {
            var time = new List<int>() { h, m, s };
            saveTimes.Add(time);
        }
        if (lap == 3)
        {
            var time = new List<int>() { h, m, s };
            saveTimes.Add(time);
        }
        if (lap == 4)
        {
            var time = new List<int>() { h, m, s };
            saveTimes.Add(time);
        }
    }


    public int Dist()
    {
        int x = 0;
        for (int i = _carModel.currentWay; i < _carModel._waypointsList.Count - 1; i++)
        {
            x += (int)Vector3.Distance(_carModel._waypointsList[_carModel.currentWay].position, _carModel._waypointsList[_carModel.currentWay + 1].position);
        }
        x += (int)Vector3.Distance(_carModel._waypointsList[_carModel.currentWay].position, transform.position);
        return x;
    }

}
