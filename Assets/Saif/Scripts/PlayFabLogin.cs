using UnityEngine;
using UnityEditor;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System;
using System.Collections.Generic;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private string username;
    [SerializeField] private int mmr = 50;
    [SerializeField] private int defaultMMR = 50;
    [SerializeField] private TMP_InputField userNameField;
    [SerializeField] private TextMeshProUGUI mmrText;
    [SerializeField] private PlayFabLeaderBoard myLeaderBoard;



    //public static Action<string> OnPlayfabLoginSuccess;

    #region Unity Methods

    private void Awake()
    {
        username = PlayerPrefs.GetString("USERNAME");
        if (string.IsNullOrEmpty(username))
        {
            userNameField.text = "Player " + UnityEngine.Random.Range(1000, 10000);
        }
        else 
        {
            userNameField.text = username;
        }
        
    }


    public void GetMMr()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnFailure);
    }

    private void OnDataReceived(GetUserDataResult result)
    {
        Debug.Log("Received User Data");
        if (result.Data != null && result.Data.ContainsKey("MMR"))
        {
            mmr = int.Parse(result.Data["MMR"].Value);
            mmrText.text = mmr.ToString();
            myLeaderBoard.SendLeaderBoard(mmr);
        }
        else 
        {
            SaveMMR(defaultMMR);
        }
    }

    public void SaveMMR(int ranking)
    {
        mmr = defaultMMR;
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"MMR", ranking.ToString()}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnFailure);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveMMR(mmr + 50);
        }
#endif
    }


    private void OnDataSend(UpdateUserDataResult obj)
    {
        //mmr = 50;
        mmrText.text = mmr.ToString();
        myLeaderBoard.SendLeaderBoard(mmr);
    }


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

        UIManager.instance.CloseAndOpenPanel(UIManager.instance.LoginPanel, UIManager.instance.MainMenuPanel);
        //OnPlayfabLoginSuccess?.Invoke(displayName);
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

    public void OnClickBack()
    {
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.MainMenuPanel, UIManager.instance.LoginPanel);
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
        GetMMr();
        UpdateDisplayName(username);


    }

    private void OnDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("You have updated the display name of playfab account!");
        //SceneController.LoadScene("PhotonScene");

    }


   

    #endregion

}
