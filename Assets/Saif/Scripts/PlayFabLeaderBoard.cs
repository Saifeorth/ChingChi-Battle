using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLeaderBoard : MonoBehaviour
{

    private List<LeaderboardEntry> playerEntries;
    [SerializeField]
    private LeaderboardEntry LeaderBoardEntryPrefab;
    [SerializeField]
    private Transform LeaderBoardContent;
    private void Awake()
    {
        playerEntries = new List<LeaderboardEntry>();
        SpawnLeaderBoardEntries();
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


    private void SpawnLeaderBoardEntries()
    {
        for (int i = 0; i < 10; i++)
        {
            LeaderboardEntry entry = Instantiate(LeaderBoardEntryPrefab, LeaderBoardContent);
        }
    }



    private void Update()
    {

        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SendLeaderBoard(50);
        }
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
        UIManager.instance.CloseAndOpenPanel(UIManager.instance.LeaderboardPanel, UIManager.instance.SelectionPanel);
    }

    void OnLeaderBoardGet(GetLeaderboardResult result) {

        UIManager.instance.CloseAndOpenPanel(UIManager.instance.SelectionPanel, UIManager.instance.LeaderboardPanel);
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
        }
    }
}
