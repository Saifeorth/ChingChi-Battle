using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using TMPro;
using ExitGames.Client.Photon;


public class PhotonMatchmaking : MonoBehaviourPunCallbacks
{


    public TMP_InputField RoomNameInputField;
    public TMP_InputField MaxPlayersInputField;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;

    public GameObject PlayerListEntryPrefab;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;



    #region Public Functions


    public void OnClickCreateRoomButton()
    {
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.SelectionPanel, UIManager.instance.CreateRoomPanel);
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.SelectionPanel, UIManager.instance.JoinRandomRoomPanel);
        PhotonNetwork.JoinRandomRoom();
    }


    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.SelectionPanel, UIManager.instance.RoomlistPanel);
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }


    

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.CreateRoomPanel, UIManager.instance.SelectionPanel);
    }


    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        //PhotonNetwork.LoadLevel("DemoAsteroids-GameScene");
    }

    public void OnBackFromRoomListPanel()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.RoomlistPanel, UIManager.instance.SelectionPanel);
    }


    public void LocalPlayerPropertiesUpdated()
    {
        UIManager.instance.startMultiplayerGameButton.SetActive(CheckPlayersReady());
    }


    #endregion



    #region Unity Functins


    private void Awake()
    {
       // PlayFabLogin.OnPlayfabLoginSuccess += ConnectToPhoton;
        PhotonNetwork.AutomaticallySyncScene = true;
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
    }


    private void OnDestroy()
    {
       // PlayFabLogin.OnPlayfabLoginSuccess -= ConnectToPhoton;
    }


    #endregion

    #region PhotonCallbacks

    public override void OnConnectedToMaster()
    {
        //print("Called");
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.LoginPanel, UIManager.instance.SelectionPanel);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnJoinedLobby()
    {
        // whenever this joins a new lobby, clear any previous room lists
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }


    public void OnClickBackFromOnline()
    {
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.SelectionPanel, UIManager.instance.MainMenuPanel);
    }

    //public override void OnCreatedRoom()
    //{
    //    UIManager.instance.CloseAndOpenPanel(UIManager.instance.CreateRoomPanel, null);
    //}






    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //SetActivePanel(SelectionPanel.name);
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.CreateRoomPanel, UIManager.instance.SelectionPanel);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //UIManager.instance.CloseAndOpenPanel(UIManager.instance.JoinRandomRoomPanel, null);
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.RoomlistPanel, UIManager.instance.SelectionPanel);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        

        string roomName = "Room " + Random.Range(1000, 10000);

        RoomOptions options = new RoomOptions { MaxPlayers = 8 };

        PhotonNetwork.CreateRoom(roomName, options, null);
        //UIManager.instance.CloseAndOpenPanel(UIManager.instance.JoinRandomRoomPanel, UIManager.instance.SelectionPanel);
    }

    public override void OnJoinedRoom()
    {
        // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
        cachedRoomList.Clear();
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.RoomlistPanel, null);
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.JoinRandomRoomPanel, UIManager.instance.InsideRoomPanel);
        //SetActivePanel(InsideRoomPanel.name);

        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(UIManager.instance.InsideRoomPanel.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerListEntry>().Initialize(p.ActorNumber, p.NickName);

            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(ChingchiGame.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
            }

            playerListEntries.Add(p.ActorNumber, entry);
        }

        UIManager.instance.startMultiplayerGameButton.SetActive(CheckPlayersReady());

        Hashtable props = new Hashtable
            {
                {ChingchiGame.PLAYER_LOADED_LEVEL, false}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }


     public override void OnLeftRoom()
        {
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.InsideRoomPanel, UIManager.instance.SelectionPanel);

            foreach (GameObject entry in playerListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            playerListEntries.Clear();
            playerListEntries = null;
        }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(PlayerListEntryPrefab);
        entry.transform.SetParent(UIManager.instance.InsideRoomPanel.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerListEntries.Add(newPlayer.ActorNumber, entry);
        UIManager.instance.startMultiplayerGameButton.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);

        UIManager.instance.startMultiplayerGameButton.SetActive(CheckPlayersReady());
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            UIManager.instance.startMultiplayerGameButton.SetActive(CheckPlayersReady());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue(ChingchiGame.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
            }
        }

        UIManager.instance.startMultiplayerGameButton.SetActive(CheckPlayersReady());
    }




    #endregion



    #region Private Functions

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }
    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(ChingchiGame.PLAYER_READY, out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }



    public void OnCreateRoomButtonClicked()
    {
        string roomName = RoomNameInputField.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

        byte maxPlayers;
        byte.TryParse(MaxPlayersInputField.text, out maxPlayers);
        maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };

        PhotonNetwork.CreateRoom(roomName, options, null);
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.CreateRoomPanel, null);
    }



    private void ConnectToPhoton(string playerName)
    {
        //string playerName = PlayerNameInput.text;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }
    }


    #endregion


}
