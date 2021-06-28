using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TMP_Text roomName;
    public TMP_Text waitPlayerTxt;
    public TMP_Text winMsj;
    public TMP_Text debugTxt;
    public TMP_Text positionFinalesNames;
    public TMP_Text positionFinalesTimes;
    public TMP_Text ViewPlayer;
    public GameObject camOne;
    bool triger;
    public bool finishRace;
    public string nameWin;
    public GameObject finishRaceMenu;
    public GameObject miniMap;
    bool online;
    public List<string> ganadores = new List<string>();
    public List<string> ganadoresTime = new List<string>();
    public Dictionary<int, string> winers = new Dictionary<int, string>();
    public GameObject cameraWin;
    int currentCam = 0;
    public GameObject created, join;
    public Image btnCreated, btnJoin;
    int currMenu;
    public Color onColor, offColor;
    public TMP_Text playerList;
    public List<GameObject> viewPlayerInLobby = new List<GameObject>();
    public GameObject LastLap;
    public GameObject selectPjMenuBtn;
    public Sprite on;
    public Sprite off;
    public TMP_Text waitHostTxt;
    Toggle _privateToggle;
    public GameObject privatePublicObj;
    int _pjPick;


    public static GameManager Instance { get; set; }

    public bool Online
    {
        get
        {
            return online;
        }
    }



    private void Awake()
    {
        if (Instance == null)        
            Instance = this;

        if(SceneManager.GetActiveScene().name == "Lobby")
         _privateToggle = privatePublicObj.GetComponentInChildren<Toggle>();

    }

   

    private void Start()
    {

        if (camOne != null && (SceneManager.GetActiveScene().name=="TrackOne"|| SceneManager.GetActiveScene().name == "TrackTwo"))
        {
            camOne.SetActive(true);
            cameraWin.SetActive(false);
        }

        if(waitPlayerTxt != null) 
            waitPlayerTxt.gameObject.SetActive(false);
        if (finishRaceMenu != null)
            finishRaceMenu.SetActive(false);
        if (miniMap != null)
            miniMap.SetActive(false);
        
        //if (SceneManager.GetActiveScene().buildIndex != 0  && SceneManager.GetActiveScene().buildIndex != 1  && SceneManager.GetActiveScene().buildIndex != 2 && SceneManager.GetActiveScene().buildIndex != 3)
        if (SceneManager.GetActiveScene().name == "TrackOne" || SceneManager.GetActiveScene().name == "TrackTwo")
        {
            NetManager.Instance.SetLaps();
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(PhotonNetwork.NickName))
                _pjPick = (int)PhotonNetwork.CurrentRoom.CustomProperties[PhotonNetwork.NickName];
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            created.SetActive(true);
            join.SetActive(false);
            btnCreated.color = onColor;
            btnJoin.color = offColor;
        }


        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.CurrentRoom.IsVisible)
                    _privateToggle.isOn = false;
                else
                    _privateToggle.isOn = true;
            }
            else
            {
                privatePublicObj.SetActive(false);
            }

        }       
    }


    public string GetCurrentLvl
    {
        get
        {
            return SceneManager.GetActiveScene().name;
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }



    void Update()
    {

        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                selectPjMenuBtn.SetActive(true);
                if (PhotonNetwork.PlayerList.Length > 1)
                {
                    selectPjMenuBtn.SetActive(true);
                    waitHostTxt.text = "";
                }
                else
                {
                    if (!Application.isEditor)
                    {
                        selectPjMenuBtn.SetActive(false);
                        waitHostTxt.text = "Wait  Players";
                    }
                }


                if (_privateToggle.isOn)
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                else
                    PhotonNetwork.CurrentRoom.IsVisible = true;
            }
            else
            {
                selectPjMenuBtn.SetActive(false);
                waitHostTxt.text = "Wait  Host";
            }

            foreach (var item in viewPlayerInLobby)
            {
                if (item.GetComponentInChildren<TMP_Text>().text == "")
                    item.GetComponent<Image>().sprite = off;
                else
                    item.GetComponent<Image>().sprite = on;
            }
            
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        
        if(roomName != null)
            roomName.text = $"{PhotonNetwork.CurrentRoom.Name}";

        if (online || SceneManager.GetActiveScene().name == "SelectPj")
        {
            if (PhotonNetwork.PlayerList.Length < 2)
            {
                if (waitPlayerTxt != null && SceneManager.GetActiveScene().name != "SelectPj")
                {
                    waitPlayerTxt.gameObject.SetActive(true);
                    waitPlayerTxt.text = "Wait  players";
                }

                /*if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(PhotonNetwork.NickName))
                    _pjPick = (int)PhotonNetwork.CurrentRoom.CustomProperties[PhotonNetwork.NickName];*/
            }
            else
            {
                if (waitPlayerTxt != null)
                {
                    waitPlayerTxt.text = "Wait  players";
                    waitPlayerTxt.gameObject.SetActive(false);
                   

                }
            }
        }




        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex > 2)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
        }


        if (SceneManager.GetActiveScene().name == "TrackOne" || SceneManager.GetActiveScene().name == "TrackTwo")
        {
            online = true;
        }


        if (finishRace)
        {
            winMsj.text = $"qualifying";

            if (RaceManager.Instance.canvas != null)
                RaceManager.Instance.canvas.SetActive(false);

            if (RaceManager.Instance.canvasFinishRace != null)
            {
                RaceManager.Instance.canvasFinishRace.SetActive(true);
                finishRaceMenu.SetActive(true);
            }


            SpectMode();
            switch (ganadores.Count)
            {
                case 1:
                    positionFinalesNames.text = 
                    $"{1}° {ganadores[0]}\n";
                    //  /////////////
                    positionFinalesTimes.text =
                   $"{ganadoresTime[0]}\n";
                    break;
                case 2:
                    positionFinalesNames.text = 
                    $"{1}° {ganadores[0]}\n" +
                    $"{2}° {ganadores[1]}\n";
                    //  /////////////
                    positionFinalesTimes.text =
                    $"{ganadoresTime[0]}\n" +
                    $"{ganadoresTime[1]}\n";
                    break;
                case 3:
                    positionFinalesNames.text = 
                    $"{1}° {ganadores[0]}\n" +
                    $"{2}° {ganadores[1]}\n" +
                    $"{3}° {ganadores[2]}\n";
                    //  /////////////
                    positionFinalesTimes.text = 
                    $"{ganadoresTime[0]}\n" +
                    $"{ganadoresTime[1]}\n" +
                    $"{ganadoresTime[2]}\n";
                    break;
                case 4:
                    positionFinalesNames.text =
                    $"{1}° {ganadores[0]}\n" +
                    $"{2}° {ganadores[1]}\n" +
                    $"{3}° {ganadores[2]}\n" +
                    $"{4}° {ganadores[3]}\n";
                    //  /////////////
                    positionFinalesTimes.text =
                    $"{ganadoresTime[0]}\n" +
                    $"{ganadoresTime[1]}\n" +
                    $"{ganadoresTime[2]}\n" +
                    $"{ganadoresTime[3]}\n";
                    break;
                case 5:
                    positionFinalesNames.text =
                    $"{1}° {ganadores[0]}\n" +
                    $"{2}° {ganadores[1]}\n" +
                    $"{3}° {ganadores[2]}\n" +
                    $"{4}° {ganadores[3]}\n" +
                    $"{5}° {ganadores[4]}\n";
                    //  /////////////
                    positionFinalesTimes.text =
                    $"{ganadoresTime[0]}\n" +
                    $"{ganadoresTime[1]}\n" +
                    $"{ganadoresTime[2]}\n" +
                    $"{ganadoresTime[3]}\n" +
                    $"{ganadoresTime[4]}\n";
                    break;
                case 6:
                    positionFinalesNames.text =
                    $"{1}° {ganadores[0]}\n" +
                    $"{2}° {ganadores[1]}\n" +
                    $"{3}° {ganadores[2]}\n" +
                    $"{4}° {ganadores[3]}\n" +
                    $"{5}° {ganadores[4]}\n" +
                    $"{6}° {ganadores[5]}\n";
                    //  /////////////
                    positionFinalesTimes.text =
                    $"{ganadoresTime[0]}\n" +
                    $"{ganadoresTime[1]}\n" +
                    $"{ganadoresTime[2]}\n" +
                    $"{ganadoresTime[3]}\n" +
                    $"{ganadoresTime[4]}\n" +
                    $"{ganadoresTime[5]}\n";
                    break;               
                default:
                    break;
            }
        }

        PlayerList();
    }


    List<CarModel> carsCam = new List<CarModel>();
    int curr = 0;
    public void SpectMode()
    {        
        if (carsCam.Any() && ganadores.Count < carsCam.Count)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || (ganadores.Contains(carsCam[curr].nickName) && carsCam[curr].nickName != PhotonNetwork.NickName))
            {
                if (curr < carsCam.Count - 1) 
                    curr++;
                else
                    curr = 0;
            }

            Debug.Log($"MIRANDO A : {carsCam[curr].nickName} !!!!");

            ViewPlayer.text = $"SPECTED : \n" +
                $" {carsCam[curr].nickName}";

            camOne.GetComponent<CamerMovement>().SetCar(carsCam[curr]);
            camOne.GetComponent<CamerMovement>().SetCameraPos(carsCam[curr].cameraPos.transform);
            camOne.GetComponent<CamerMovement>().SetViewPoint(carsCam[curr].camViewPoint.transform);
            camOne.GetComponent<CamerMovement>().SetCameraLookAt(carsCam[curr].camLookAt.transform);
        }
    }


    public void LoadSelectPjMenu()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        
        PhotonNetwork.LoadLevel("SelectPj");
    }


    public void LoadLevel()
    {
        
        switch (PhotonNetwork.CurrentRoom.CustomProperties["Track"])
        {
            case 1:
                PhotonNetwork.LoadLevel("TrackOne");
                break;
            case 2:
                PhotonNetwork.LoadLevel("TrackTwo");
                break;
            default:
                PhotonNetwork.LoadLevel("TrackOne");
                break;
        }

    }
    

    public void Replay()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        else
            debugTxt.text = "wait for host";
    }


    public void SwichMenu()
    {
        if (SceneManager.GetActiveScene().name == "MenuOnline")
            SceneManager.LoadScene("Menu");
        if (SceneManager.GetActiveScene().name == "Menu")
            SceneManager.LoadScene("MenuOnline");
    }


    public void GoCreateMenu()
    {
        created.SetActive(true);
        join.SetActive(false);
        btnCreated.color = onColor;
        btnJoin.color = offColor;
    }

    public void GoJoinMenu()
    {
        created.SetActive(false);
        join.SetActive(true);
        btnCreated.color = offColor;
        btnJoin.color = onColor;
    }

    public void GoFindMenu()
    {
        created.SetActive(false);
        join.SetActive(false);
    }

    public void GoMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Menu");
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayerList()
    {
        if (SceneManager.GetActiveScene().name == "TrackOne" || SceneManager.GetActiveScene().name == "TrackTwo" || SceneManager.GetActiveScene().name == "Lobby")
        {
            string list = "<size=0.55>Players in Party</size>\n";

            foreach (var item in PhotonNetwork.CurrentRoom.Players.Values)
            {
                list += $"\n {item.NickName}";
            }

            if (playerList != null)
                playerList.text = list;

            if(SceneManager.GetActiveScene().name == "Lobby")
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    viewPlayerInLobby[i].GetComponentInChildren<TMP_Text>().text = PhotonNetwork.PlayerList[i].NickName;
                }
            }
            else
            {
                if (carsCam.Count < PhotonNetwork.PlayerList.Length)
                {
                    foreach (var item in FindObjectsOfType<CarModel>())
                    {
                        if (!carsCam.Contains(item))
                            carsCam.Add(item);
                    }
                }
            }
        }
    }
}
