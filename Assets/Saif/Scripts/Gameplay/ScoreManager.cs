using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;

public class ScoreManager : MonoBehaviour
{
    public List<ChingChiCharacter> allCharacters;
    public List<Stats> gameStats = new List<Stats>();


    private List<Stats> endStats = new List<Stats>();

    [SerializeField]
    private int mmr = 0;


    [SerializeField]
    private TextMeshProUGUI[] serialNumberText;
    [SerializeField]
    private TextMeshProUGUI[] playerNamesTexts;
    [SerializeField]
    private TextMeshProUGUI[] killsTexts;
    [SerializeField]
    private TextMeshProUGUI[] deathsTexts;


    public GameObject mainButton;



    private void OnEnable()
    {
        GampeplayUIManager.OnTimeEnded += UpdateStats;
    }


    private void OnDisable()
    {
        GampeplayUIManager.OnTimeEnded -= UpdateStats;
    }



    private void Start()
    {
        allCharacters = new List<ChingChiCharacter>();
        mmr = PlayerPrefs.GetInt("MMR");
        mainButton.SetActive(false);
        gameStats = new List<Stats>();
    }


    [System.Serializable]
    public struct Stats 
    {
        public ChingChiCharacter character;
        public string characterName;
        public int kills;
        public int deaths;    
    }




    public void AddStats(ChingChiCharacter player)
    {
        allCharacters.Add(player);
    }



    public void UpdateStats()
    {
        for (int i = 0; i < allCharacters.Count; i++)
        {
            Stats stat = new Stats();
            stat.characterName = allCharacters[i].GetName();
            stat.kills = allCharacters[i].Kills;
            stat.deaths = allCharacters[i].Deaths;
            stat.character = allCharacters[i];
            gameStats.Add(stat);
        }

        SortRanking();
    }


    public void SortRanking()
    {
        endStats = gameStats.OrderByDescending(Stats => Stats.kills).ThenBy(Stats => Stats.kills).ToList<Stats>();

        for (int i = 0; i < gameStats.Count; i++)
        {
            serialNumberText[i].text = i.ToString();
            playerNamesTexts[i].text = endStats[i].characterName;
            killsTexts[i].text = endStats[i].kills.ToString();
            deathsTexts[i].text = endStats[i].deaths.ToString();
        }

        AssignRanking();
    }


    public void AssignRanking()
    {
        if (endStats[0].character.IsPlayable())
        {
            mmr += 30;
        }

        if (endStats[1].character.IsPlayable())
        {
            mmr += 15;
        }

        if (endStats[1].character.IsPlayable())
        {
            mmr += 5;
        }


        SaveMMR(mmr);
    }

    public void SaveMMR(int ranking)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"MMR", ranking.ToString()}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnFailure);
    }


    private void OnDataSend(UpdateUserDataResult obj)
    {
        //mmr = 50;
        //mmrText.text = mmr.ToString();
        //myLeaderBoard.SendLeaderBoard(mmr);
        PlayerPrefs.SetInt("MMR", mmr);
        SendLeaderBoard(mmr);
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
        mainButton.SetActive(true);
    }


    private void OnFailure(PlayFabError error)
    {
        Debug.Log($"There was an issue with your request {error.GenerateErrorReport()}");
    }



















}
