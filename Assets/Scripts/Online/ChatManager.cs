using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;


public class ChatManager : MonoBehaviour, IChatClientListener
{


    ChatClient _client;
    string _channelRoom;
    public TMP_InputField inputField;
    public TextMeshProUGUI content;
    public ScrollRect scroll;
    public Button openChat, hidenChat;
    public GameObject chatContent, notyChat;
    public TextMeshProUGUI cantMsj;
    int cant;
    bool typing;


    public bool Typing
    {
        get
        {
            return typing;
        }
    }

    void Start()
    {
        _client = new ChatClient(this);
        _client.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(PhotonNetwork.NickName));
        _channelRoom = PhotonNetwork.CurrentRoom.Name;
        chatContent.SetActive(false);
        notyChat.SetActive(true);
    }


    public void OpenChat()
    {
        if (chatContent.activeSelf)
        {
            chatContent.SetActive(false);
            notyChat.SetActive(true);
        }
        else if (notyChat.activeSelf)
        {
            chatContent.SetActive(true);
            notyChat.SetActive(false);
            cant = 0;
        }
    }

    public void Clear()
    {
        content.text = "";
        cant = 0;
    }

    void Update()
    {
        _client.Service();

        if (chatContent.activeSelf)
        {
            inputField.Select();
        }

        cantMsj.text = cant != 0 ? cantMsj.text = cant.ToString() : "";
    }

    public void OnConnected()
    {
        Debug.Log("ENTER CHAT");
        _client.Subscribe(_channelRoom);
    }


    public void SendMessages()
    {
        var msj = inputField.text;

        if (string.IsNullOrEmpty(inputField.text) || string.IsNullOrWhiteSpace(inputField.text)) return;

        string[] m = msj.Split(' ');
        if (m.Length < 3)
        {
            _client.PublishMessage(_channelRoom, inputField.text); 
            inputField.text = "";
            return;
        }
        else if (m[0] != "/w")
        {
            _client.PublishMessage(_channelRoom, inputField.text); 
            inputField.text = "";
            return;
        }
        else
        {
            _client.SendPrivateMessage(m[1], string.Join("", m, 2, m.Length - 2));
            inputField.text = $"/w {m[1]}";
        }


    }


    public void OnDisconnected()
    {
        Debug.Log("SALI DEL CHAT");
    }



    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Debug.Log("ENTRO MENSAJE");

        for (int i = 0; i < senders.Length; i++)
        {
            var user = senders[i];
            var mess = messages[i];
            string tagColor;
            string messColor;

            if (PhotonNetwork.NickName == user)
            {
                tagColor = "orange";
                messColor = "white";
            }
            else
            {
                tagColor = "green";
                messColor = "white";
            }
            content.text += $"\n<color={tagColor}> [{user}] : </color> <color={messColor}> {mess}</color>";
            cant++;
        }

        AutoScroll();
    }


    void AutoScroll()
    {
        if (scroll.verticalNormalizedPosition < .2f)
            StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
    {
        yield return new WaitForEndOfFrame();
        scroll.verticalNormalizedPosition = 0;
    }


    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        content.text += $"\n <color=yellow> private  [{sender}] : </color> <color=yellow> {message} </color>";
        cant++;
        AutoScroll();
    }


    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnChatStateChange(ChatState state)
    {

    }


    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

    }

    public void OnSubscribed(string[] channels, bool[] results)
    {

    }

    public void OnUnsubscribed(string[] channels)
    {

    }

    public void OnUserSubscribed(string channel, string user)
    {

    }

    public void OnUserUnsubscribed(string channel, string user)
    {

    }





}
