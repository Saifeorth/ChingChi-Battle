using System;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class UIFriend : MonoBehaviour
{
    [SerializeField] private TMP_Text friendNameText;
    [SerializeField] private FriendInfo friend;


    public static Action<string> OnRemoveFriend = delegate { };

    public void Inititalize(FriendInfo friendInfo)
    {
        this.friend = friendInfo;
        friendNameText.SetText(this.friend.UserId);
    }

    public void RemoveFriend()
    {
        OnRemoveFriend?.Invoke(friend.UserId);
    }

}
