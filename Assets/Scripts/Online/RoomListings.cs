using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;


public class RoomListings : MonoBehaviourPunCallbacks
{

    public Transform content;
    public RoomItem roomListings;
    List<RoomItem> _roomsList = new List<RoomItem>();

    List<string> namesRoom = new List<string>();


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = _roomsList.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(_roomsList[index].gameObject);
                    namesRoom.Remove(_roomsList[index].name);
                    _roomsList.RemoveAt(index);
                }
            }
            else
            {
                if (info.IsVisible)
                {
                    if (!namesRoom.Contains(info.Name))
                    {
                        RoomItem room = Instantiate(roomListings, content);
                        if (room != null)
                        {
                            room.SetRoomInfo(info);
                            _roomsList.Add(room);
                            namesRoom.Add(info.Name);
                        }
                    }
                }
            }
        }
    }


}
