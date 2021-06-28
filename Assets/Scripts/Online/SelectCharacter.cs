using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class SelectCharacter : MonoBehaviourPun
{


    
    public List<GameObject> cars = new List<GameObject>();
    public int curr;
    public Vector2 currVec;


    Dictionary<int, GameObject> carsDic = new Dictionary<int, GameObject>();
    public List<bool> carsPick = new List<bool>();
    public GameObject confirmBtn;
    public GameObject nextBtn;
    public GameObject prevBtb;
    public List<GameObject> OthersPj = new List<GameObject>();
    public List<GameObject> outline = new List<GameObject>();
    bool ready;


    public Image accelScroolBar;
    public Image turnScroolBar;
    public Image speedScroolBar;

    public VideoPlayer videoPlayer;

    public GameObject startBtn;

    int countPick;

    public GameObject waitPlayerTxt;
    
    public TMP_Text name, description;


    public GameObject descriptionObj, videoObj;


    public AudioClip sound;
    private AudioSource source;


    public static SelectCharacter Instance { get; }




    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        confirmBtn.SetActive(false);
        waitPlayerTxt.SetActive(false);

        for (int i = 0; i < cars.Count; i++)
        {
            if (i == curr)
                cars[curr].SetActive(true);
            else
                cars[i].SetActive(false);

            carsDic.Add(i, cars[i]);
            carsPick.Add(false);
        }

        foreach (var item in OthersPj)
        {
            item.SetActive(false);
        }
    }



    public bool AllPlayerReady
    {
        get
        {
            return carsPick[curr];
        }
    }

    bool cantSelect;

    void Update()
    {
        transform.Rotate(Vector3.up, .1f);


        if (PhotonNetwork.IsMasterClient && ready && countPick == PhotonNetwork.PlayerList.Length && startBtn != null)
        {
            //startBtn.SetActive(true);
            //waitPlayerTxt.SetActive(false);
            
            if (countPick > 1)
            {
                startBtn.SetActive(true);
                waitPlayerTxt.SetActive(false);
            }
            else
            {
                if(Application.isEditor) startBtn.SetActive(true);
                waitPlayerTxt.SetActive(true);
            }
            
        }
        else if(PhotonNetwork.IsMasterClient && ready && countPick != PhotonNetwork.PlayerList.Length)
        {
            waitPlayerTxt.SetActive(true);
            startBtn.SetActive(false);
        }



        if (ready) return;

        if (carsPick[curr])
        {
            confirmBtn.SetActive(false);
            outline[curr].GetComponent<Image>().color = Color.red;

            if (!source.isPlaying && !cantSelect)
            {
                source.clip = sound;
                source.Play();
                cantSelect = true;
            }
        }
        else
        {
            cantSelect = false;
            source.Stop();
            confirmBtn.SetActive(true);
            outline[curr].GetComponent<Image>().color = Color.green;
        }

        for (int i = 0; i < carsDic.Count; i++)
        {
            if(i == curr)
            {
                carsDic[curr].SetActive(true);
                outline[i].SetActive(true);
            }
            else
            {
                carsDic[i].SetActive(false);
                outline[i].SetActive(false);
            }
        }


        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.mouseScrollDelta.normalized ==  new Vector2(0,-1))
            PrevPj();
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.mouseScrollDelta.normalized == new Vector2(0, 1))
            NextPj();
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            UpPj();
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            DownPj();


        speedScroolBar.fillAmount = carsDic[curr].GetComponent<StatsView>().statsData.maxPower / 1800;
        accelScroolBar.fillAmount = carsDic[curr].GetComponent<StatsView>().statsData.accelFactor / .3f;
        turnScroolBar.fillAmount = carsDic[curr].GetComponent<StatsView>().statsData.trurnFactor / .3f;
        videoPlayer.clip = carsDic[curr].GetComponent<StatsView>().statsData.clip;

        name.text = carsDic[curr].GetComponent<StatsView>().statsData._name;
        description.text = carsDic[curr].GetComponent<StatsView>().statsData.Desctiption;
    }



    public void NextPj()
    {   
        switch (currVec)
        {
            case Vector2 v when v.Equals(new Vector2(0,0)):
                currVec = new Vector2(0, 1);
                break;
            case Vector2 v when v.Equals(new Vector2(0, 1)):
                currVec = new Vector2(1, 0);
                break;
            case Vector2 v when v.Equals(new Vector2(1, 0)):
                currVec = new Vector2(1, 1);
                break;
            case Vector2 v when v.Equals(new Vector2(1, 1)):
                currVec = new Vector2(2, 0);
                break;
            case Vector2 v when v.Equals(new Vector2(2, 0)):
                currVec = new Vector2(2, 1);
                break;
            case Vector2 v when v.Equals(new Vector2(2, 1)):
                currVec = new Vector2(0, 0);
                break;
        }
        curr = (int)currVec.y + (int)currVec.x * 2;
    }

    public void PrevPj()
    {  
        switch (currVec)
        {
            case Vector2 v when v.Equals(new Vector2(0, 0)):
                currVec = new Vector2(2, 1);
                break;
            case Vector2 v when v.Equals(new Vector2(2, 1)):
                currVec = new Vector2(2, 0);
                break;
            case Vector2 v when v.Equals(new Vector2(2, 0)):
                currVec = new Vector2(1, 1);
                break;
            case Vector2 v when v.Equals(new Vector2(1, 1)):
                currVec = new Vector2(1, 0);
                break;
            case Vector2 v when v.Equals(new Vector2(1, 0)):
                currVec = new Vector2(0, 1);
                break;
            case Vector2 v when v.Equals(new Vector2(0, 1)):
                currVec = new Vector2(0, 0);
                break;
        }
        curr = (int)currVec.y + (int)currVec.x * 2;
    }

    public void UpPj()
    {
        switch (currVec)
        {
            case Vector2 vec when vec.Equals(new Vector2(0, 0)):
                currVec = new Vector2(2, 0);
                break;
            case Vector2 vec when vec.Equals(new Vector2(0, 1)):
                currVec = new Vector2(2, 1);
                break;
            case Vector2 vec when vec.Equals(new Vector2(1, 0)):
                currVec = new Vector2(0, 0);
                break;
            case Vector2 vec when vec.Equals(new Vector2(1, 1)):
                currVec = new Vector2(0, 1);
                break;
            case Vector2 vec when vec.Equals(new Vector2(2, 0)):
                currVec = new Vector2(1, 0);
                break;
            case Vector2 vec when vec.Equals(new Vector2(2, 1)):
                currVec = new Vector2(1, 1);
                break;

            default:
                currVec = new Vector2(0, 0);
                break;
        }
        curr = (int)currVec.y + (int)currVec.x * 2;
    }

    public void DownPj()
    {
        switch (currVec)
        {
            case Vector2 vec when vec.Equals(new Vector2(0, 0)):
                currVec = new Vector2(1, 0);
                break;
            case Vector2 vec when vec.Equals(new Vector2(0, 1)):
                currVec = new Vector2(1, 1);
                break;
            case Vector2 vec when vec.Equals(new Vector2(1, 0)):
                currVec = new Vector2(2, 0);
                break;
            case Vector2 vec when vec.Equals(new Vector2(1, 1)):
                currVec = new Vector2(2, 1);
                break;
            case Vector2 vec when vec.Equals(new Vector2(2, 0)):
                currVec = new Vector2(0, 0);
                break;
            case Vector2 vec when vec.Equals(new Vector2(2, 1)):
                currVec = new Vector2(0, 1);
                break;
        }

        curr = (int)currVec.y + (int)currVec.x * 2;
    }

    public void Confirm()
    {
        photonView.RPC("SetPick", RpcTarget.AllBuffered, curr, true);
        confirmBtn.SetActive(false);
        nextBtn.SetActive(false);
        prevBtb.SetActive(false);
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(PhotonNetwork.NickName))
        {
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;
            hash.Add(PhotonNetwork.NickName, curr);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }
        ready = true;
        foreach (var item in outline)
        {
            item.SetActive(false);
        }
    }

    public void Select(int n)
    {
        curr = n;
    }


    [PunRPC]
    public void SetPick(int curr, bool b)
    {
        countPick++;
        carsPick[curr] = b;
        OthersPj[curr].SetActive(b);
        
    }


    bool descript;

    public  void Switchup()
    {
        if (descript)
        {
            descriptionObj.SetActive(true);
            videoObj.SetActive(false);
            descript = !descript;
        }
        else
        {
            descriptionObj.SetActive(false);
            videoObj.SetActive(true);
            descript = !descript;
        }
    }


}
