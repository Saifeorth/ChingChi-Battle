using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;


public class PlayFabLeaderBoard : MonoBehaviour
{

    [SerializeField]
    private List<LeaderboardEntry> playerEntries;
    [SerializeField]
    private LeaderboardEntry LeaderBoardEntryPrefab;


    [SerializeField]
    private Transform LeaderBoardContent;
    private void Awake()
    {
        playerEntries = new List<LeaderboardEntry>();
        //SpawnLeaderBoardEntries();
    }


    public void SendLeaderBoard(int MMR)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "MMR",
                    Value = MMR
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnFailure);
    }

    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful LeaderBoard Sent");


    }

    private void OnFailure(PlayFabError error)
    {
        Debug.Log($"There was an issue with your request {error.GenerateErrorReport()}");
    }


    //private void SpawnLeaderBoardEntries()
    //{
    //    for (int i = 0; i < 10; i++)
    //    {
    //        LeaderboardEntry entry = Instantiate(LeaderBoardEntryPrefab, LeaderBoardContent);
    //    }
    //}



    private void Update()
    {

        #if UNITY_EDITOR
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    SendLeaderBoard(50);
        //}
        #endif
    }

    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "MMR",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnFailure);
    }


    public void GetBackToSelection()
    {
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.LeaderboardPanel, UIManager.instance.MainMenuPanel);
        RemoveEntries();
    }

    void OnLeaderBoardGet(GetLeaderboardResult result) {

        UIManager.instance.CloseAndOpenPanel(UIManager.instance.MainMenuPanel, UIManager.instance.LeaderboardPanel);
        foreach (var item in result.Leaderboard)
        {
            LeaderboardEntry leaderBoardEntry = Instantiate(LeaderBoardEntryPrefab, LeaderBoardContent);
            leaderBoardEntry.OnValuesUpdate(item.Position.ToString(), item.DisplayName, item.StatValue.ToString());
            playerEntries.Add(leaderBoardEntry);
            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
        }
    }


    private void RemoveEntries()
    {
        for (int i = LeaderBoardContent.childCount-1; i >=0 ; i--)
        {
            //Destroy(LeaderBoardContent.GetChild(i));
            LeaderBoardContent.GetChild(i).gameObject.SetActive(false);
        }
    }


    //[System.Serializable]
    //public struct LeaderBoadData 
    //{

    //    public GameObject LeaderBoardEntryGameObject;
    //    public TextMeshProUGUI LeaderBoardEntrySerialNumberText;
    //    public TextMeshProUGUI LeaderBoardEntryPlayerNameText;
    //    public TextMeshProUGUI LeaderBoardEntryPlayerMMRText;
    //}
}
