using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NetManager : MonoBehaviourPunCallbacks
{

    bool offline = false;
    public static NetManager Instance { get; set; }


    public int maxPlayers;
    public int maxLaps;


    public TMP_InputField username;
    public TMP_Text debugNullNickName;
    public TMP_Text debugInfo;

    public GameObject gearLoading;
    public GameObject exclamationMark;

    [Header("_____CREATE ROOM_____")]
    public TMP_InputField map;
    //public TMP_InputField cantPlayers;
    //public TMP_InputField cantLaps;
    public TMP_Dropdown cantPlayerDropDown;
    public TMP_Dropdown cantLapsDropDown;
    public TMP_Dropdown trackDropDown;
    public GameObject randomRoomBtn;
    public TMP_Text debugCreateRoom;
    public Toggle privateToggle;

    public TMP_Text maxPlayerTxt, maxLapTxt;


    [Header("_____JOIN ROOM_____")]
    public TMP_InputField nameRoomConnect;
    public GameObject joinRoomBtn;
    public TMP_Text debugJoinRoom;

    public GameObject joinRandomRoomBtn;


    public TMP_Text cantRooms;


    public bool Offline
    {
        get
        {
            return offline;
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }



    void Start()
    {
        ConnectToServer();
        exclamationMark.SetActive(false);
        gearLoading.SetActive(false);
        if (map != null && maxPlayerTxt != null && maxLapTxt)
        {
            if (maxLaps > 9) maxLaps = 9;
            if (maxPlayers > 10) maxPlayers = 10;
            map.text = "2";
            maxLapTxt.text = $"(1-{maxLaps})";
            maxPlayerTxt.text = $"(2-{ maxPlayers})";
        }
    }


    public void SetLaps()
    {
        cantLapsDropDown.captionText.text = PhotonNetwork.CurrentRoom.CustomProperties["Laps"].ToString();
    }

    private void Update()
    {

        if (cantRooms != null)
            cantRooms.text = $" Players : {PhotonNetwork.CountOfPlayers} | Rooms : {PhotonNetwork.CountOfRooms}";

        #region VALIDACION INPUTS

        if (map != null && cantPlayerDropDown != null && cantLapsDropDown != null)
            {
                if (map.text != "1" && map.text != "2")
                    map.text = "1";
                if (cantLapsDropDown.value  > maxLaps)
                    cantLapsDropDown.value = 3;
                if (cantPlayerDropDown.value > maxPlayers) 
                    cantPlayerDropDown.value = maxPlayers;
                if (cantLapsDropDown.value < 1 || cantLapsDropDown.value > maxLaps)
                    cantLapsDropDown.value = 3;

                if (cantPlayerDropDown.value < 2 || cantPlayerDropDown.value > maxPlayers)
                    cantPlayerDropDown.value = 5;
            }

            if (username != null && debugNullNickName != null && debugInfo != null  && randomRoomBtn != null && debugCreateRoom != null && nameRoomConnect != null && joinRoomBtn != null && debugJoinRoom != null)
            {
                if (username.text == "")
                {
                    debugNullNickName.text = $"Enter username";
                    //randomRoomBtn.gameObject.SetActive(false);
                    //joinRoomBtn.gameObject.SetActive(false);
                    //joinRandomRoomBtn.gameObject.SetActive(false);
                    exclamationMark.SetActive(true);
                    randomRoomBtn.GetComponent<Button>().interactable=false;
                    joinRoomBtn.GetComponent<Button>().interactable = false;
                    joinRandomRoomBtn.GetComponent<Button>().interactable = false;
            }
                else
                {
                    debugNullNickName.text = "";
                    randomRoomBtn.GetComponent<Button>().interactable = true;
                    exclamationMark.SetActive(false);
                    if (nameRoomConnect.text != "")
                        {
                        joinRoomBtn.GetComponent<Button>().interactable = true;
                        if (PhotonNetwork.CountOfRooms > 0)
                            joinRandomRoomBtn.GetComponent<Button>().interactable = true;
                    }
                }

                if (username.text != "")
                {
                    randomRoomBtn.gameObject.SetActive(true);
                    if (PhotonNetwork.CountOfRooms > 0)
                        joinRandomRoomBtn.gameObject.SetActive(true);
                }

                // connect room
                if (nameRoomConnect.text == "")
                {
                    joinRoomBtn.GetComponent<Button>().interactable = false;
                    debugJoinRoom.text = $"Enter room code";
                }
                else
                {
                    debugJoinRoom.text = "";
                    if (username.text != "")
                    {
                        joinRoomBtn.GetComponent<Button>().interactable =true;
                        //joinRandomRoomBtn.gameObject.SetActive(true);
                    }
                }
            }

            /*if(PhotonNetwork.CountOfRooms == 0)
                joinRandomRoomBtn.gameObject.SetActive(false);*/

        #endregion
    
    }


    void ConnectToServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
                if (debugInfo != null) debugInfo.text = $"\n Connect server";
                else
                if (debugInfo != null) debugInfo.text = $"\n Failing Connecting to Server";
        }
    }


    public void ChangeNickName()
    {
        if (username != null && username.text == "")
        {
            username.text = "sarasa";
            PhotonNetwork.LocalPlayer.NickName = username.text;
        }
        else if (username != null)
        {
            PhotonNetwork.LocalPlayer.NickName = username.text;
        }
    }

    void SetCantLaps()
    {
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Laps"))
        {
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;
            hash.Add("Laps", int.Parse(cantLapsDropDown.captionText.text));
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }
    }

    void SetTrack()
    {
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Track"))
        {
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;
            hash.Add("Track", int.Parse(trackDropDown.captionText.text));
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }
    }


    public void CreateToRoom()
    {     
        ChangeNickName();
        var option = new RoomOptions();
        if (cantPlayerDropDown != null)
        option.MaxPlayers = byte.Parse(cantPlayerDropDown.captionText.text);
        option.IsOpen = true;
        if (privateToggle.isOn)       
            option.IsVisible = false;
        else
            option.IsVisible = true;

        if (debugInfo != null) debugInfo.text = $"\n join room \n max players {byte.Parse(cantPlayerDropDown.captionText.text)}";
    }


    public void OfflineLevel()
    {
        offline = true;
        PhotonNetwork.LocalPlayer.NickName = "sarasa";
        CreateToRandomRoom();       
    }

    public void ConnectToRoom()
    {
        ChangeNickName();
        PhotonNetwork.JoinRoom(nameRoomConnect.text);
        joinRoomBtn.GetComponent<Button>().interactable = false;
        joinRoomBtn.GetComponentInChildren<TextMeshProUGUI>().text = "wait";
        gearLoading.SetActive(true);
        if (debugInfo != null) debugInfo.text = $"\n join room \n max players {byte.Parse(cantPlayerDropDown.captionText.text)}";
    }
   

    public void ConnectToRandomRoom()
    {
        if (PhotonNetwork.CountOfRooms > 0)
        {
            ChangeNickName();
            PhotonNetwork.JoinRandomRoom();
        }
        else
            CreateToRandomRoom();

        if (debugInfo != null) debugInfo.text = $"\n join room \n max players {byte.Parse(cantPlayerDropDown.captionText.text)}";
    }

    public void CreateToRandomRoom()
    {
        ChangeNickName();
        var roomName = Random.Range(1000, 9999).ToString();
        var option = new RoomOptions();

        if (cantPlayerDropDown != null)
            option.MaxPlayers = byte.Parse(cantPlayerDropDown.captionText.text);
        else
            option.MaxPlayers = 5;
     
        option.IsOpen = true;

        if (privateToggle.isOn)
            option.IsVisible = false;
        else
            option.IsVisible = true;
        randomRoomBtn.GetComponent<Button>().interactable = false;
        randomRoomBtn.GetComponentInChildren<TextMeshProUGUI>().text = "wait";
        gearLoading.SetActive(true);
        PhotonNetwork.JoinOrCreateRoom(roomName, option, TypedLobby.Default);
        if (debugInfo != null) debugInfo.text = $"\n join room \n max players {byte.Parse(cantPlayerDropDown.captionText.text)}";
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
        if(debugInfo != null) debugInfo.text = $"\n You are connected to the { PhotonNetwork.CloudRegion.ToUpper() } server";
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnCreatedRoom()
    {
         if(debugInfo != null) debugInfo.text = $"\n Room Created";
        Debug.Log("Room Created");
    }

    public override void OnJoinedRoom()
    {
        if (debugInfo != null) debugInfo.text = $"\n Joined Room";

         SetCantLaps();
         SetTrack();

         //PhotonNetwork.LoadLevel("SelectPj"); 
         PhotonNetwork.LoadLevel("Lobby"); 
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (debugInfo != null) debugInfo.text = $"\n Fail Joining Room \n {message}";
        joinRoomBtn.GetComponent<Button>().interactable = true;
        joinRoomBtn.GetComponentInChildren<TextMeshProUGUI>().text = "join";
        gearLoading.SetActive(false);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (debugInfo != null) debugInfo.text = $"\n { returnCode} Fail Creating Room {message}";
        CreateToRandomRoom();
        gearLoading.SetActive(false);
        randomRoomBtn.GetComponent<Button>().interactable = true;
        randomRoomBtn.GetComponentInChildren<TextMeshProUGUI>().text = "create";
    }
}
