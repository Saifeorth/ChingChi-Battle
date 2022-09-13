using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Linq;

public class PlayFabFriendController : MonoBehaviour
{
    public static Action<List<FriendInfo>> OnFriendListUpdated = delegate { };

    private List<FriendInfo> friends;

    private void Awake()
    {
        friends = new List<FriendInfo>();
        NetworkManager.GetPhotonFriends += HandleGetFriends;
        UIAddFriend.OnAddFriend += HandleAddPlayfabFriend;
        UIFriend.OnRemoveFriend += HandleRemoveFriend;
    }

    private void OnDestroy()
    {
        NetworkManager.GetPhotonFriends -= HandleGetFriends;
        UIAddFriend.OnAddFriend -= HandleAddPlayfabFriend;
        UIFriend.OnRemoveFriend -= HandleRemoveFriend;
    }

    private void HandleGetFriends()
    {
        GetPlayfabFriends();
    }

    private void HandleRemoveFriend(string name)
    {
        string id = friends.FirstOrDefault(f => f.TitleDisplayName == name).FriendPlayFabId;
        var request = new RemoveFriendRequest { FriendPlayFabId = id };
        PlayFabClientAPI.RemoveFriend(request, OnFriendRemovedSuccess, OnFailure);
    }

    private void OnFriendRemovedSuccess(RemoveFriendResult result)
    {
        GetPlayfabFriends();
    }

    private void HandleAddPlayfabFriend(string name)
    {
        var request = new AddFriendRequest { FriendTitleDisplayName = name };
        PlayFabClientAPI.AddFriend(request, OnFriendsAddedSuccess, OnFailure);
    }

    private void OnFriendsAddedSuccess(AddFriendResult result)
    {
        GetPlayfabFriends();
    }

    private void OnFailure(PlayFabError error)
    {
        Debug.Log($"Error occured {error.GenerateErrorReport()}");
    }


    private void GetPlayfabFriends()
    {
        var request = new GetFriendsListRequest { IncludeSteamFriends = false, IncludeFacebookFriends = false, XboxToken = null };
        PlayFabClientAPI.GetFriendsList(request, OnFriendsListSuccess, OnFailure);
    }

    private void OnFriendsListSuccess(GetFriendsListResult result)
    {
        friends = result.Friends;
        OnFriendListUpdated?.Invoke(result.Friends);
    }
}
