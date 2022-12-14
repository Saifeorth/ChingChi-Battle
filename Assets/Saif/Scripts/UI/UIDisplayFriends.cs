using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplayFriends : MonoBehaviour
{
    [SerializeField] private Transform friendContainer;
    [SerializeField] private UIFriend uiFriendPrefab;

    private void Awake()
    {
        PhotonFriendController.OnDisplayFriends += HandleDisplayfriends;
    }

    private void HandleDisplayfriends(List<FriendInfo> friends)
    {
        foreach (Transform child in friendContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (FriendInfo friend in friends)
        {
            UIFriend uiFriend = Instantiate(uiFriendPrefab, friendContainer);
            uiFriend.Inititalize(friend);
        }
    }

    private void OnDestroy()
    {
        PhotonFriendController.OnDisplayFriends -= HandleDisplayfriends;
    }
}
