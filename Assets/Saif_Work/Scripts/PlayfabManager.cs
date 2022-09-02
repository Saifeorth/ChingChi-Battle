using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest {

            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
    };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful Login/ Account Created");
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError("Error while Loggin in/Creating Account");
        Debug.Log(error.GenerateErrorReport());
    }

}
