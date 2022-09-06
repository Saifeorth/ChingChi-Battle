using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private string username;

    #region Unity Methods
    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "75584";
        }
    }
    #endregion


    #region Private Methods 
    private bool IsValidUsername()
    {
        bool isValid = false;
        if (username.Length >= 3 && username.Length <= 24)
        {
            isValid =  true;
        }

        return isValid;
    }

    private void LoginWithCustomId()
    {
        Debug.Log($"Logging To playfab as {username}");
        var request = new LoginWithCustomIDRequest { CustomId = username, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginCustomIdSuccess, OnFailure);
    }


    private void UpdateDisplayName(string displayName)
    {
        Debug.Log($"Updating Playfab account's display name to {displayName}");
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = displayName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameSuccess, OnFailure);
    }

   


    #endregion


    #region Public Methods
    public void SetUserName(string name)
    {
        username = name;
        PlayerPrefs.SetString("USERNAME", username);
    }


    public void Login()
    {
        if (!IsValidUsername()) return;

        LoginWithCustomId();
    }

    #endregion

    #region PlayFab Callbacks
    private void OnFailure(PlayFabError error)
    {
        Debug.Log($"There was an issue with your request {error.GenerateErrorReport()}");
    }

    private void OnLoginCustomIdSuccess(LoginResult result)
    {
        Debug.Log($"You have logged into Playfab using custom id {username}");
        UpdateDisplayName(username);

    }

    private void OnDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("You have updated the display name of playfab account!");
        SceneController.LoadScene("PhotonScene");
    }

    #endregion

}
