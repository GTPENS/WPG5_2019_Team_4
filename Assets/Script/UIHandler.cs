using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UIHandler : MonoBehaviourPunCallbacks
{
    public InputField createRoomIF;
    public InputField joinRoomIF;

    public void OnClick_CreateRoom()
    {
        PhotonNetwork.CreateRoom(createRoomIF.text, new RoomOptions { MaxPlayers = 2 }, null);
    }

    public void OnClick_JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomIF.text, null);
    }

    public override void OnJoinedRoom()
    {
        print("Room Joined Success");
        PhotonNetwork.LoadLevel("Gameplay");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Room Failed " + returnCode + "Message " + message);

    }
}
