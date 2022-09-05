using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System; 

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public static Action GetPhotonFriends = delegate { };
    // Start is called before the first frame update
    void Start()
    {
        string nickname = PlayerPrefs.GetString("USERNAME");
        ConnectToPhoton(nickname); 
    }

    private void ConnectToPhoton(string nickName)
    {
        Debug.Log($"Connected to Photon as {nickName}");
        PhotonNetwork.AuthValues = new AuthenticationValues(nickName);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = nickName;
        PhotonNetwork.ConnectUsingSettings();
    }


    private void CreatePhotonRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("You have connected to the Photon Master Server");
        if(!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("You have connected to the Photon Lobby");

        GetPhotonFriends?.Invoke();

        //CreatePhotonRoom("TestRoom");
    }


    public override void OnCreatedRoom()
    {
        Debug.Log($"You have created the Photon Room named {PhotonNetwork.CurrentRoom.Name}");

    }


    public override void OnJoinedRoom()
    {
        Debug.Log($"You have joined the Photon Room named {PhotonNetwork.CurrentRoom.Name}");
    }


    public override void OnLeftRoom()
    {
        Debug.Log("You have left a Photon Room ");
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("You have failed to joined the Photon Room ");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Another Player Has joined Room  {newPlayer.UserId}");
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Another Player Has joined Room  {otherPlayer.UserId}");
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"New Master Client Is {newMasterClient.UserId}");
    }










}
