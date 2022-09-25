using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public List<ChingChiCharacter> allCharacters;
    public List<Stats> gameStats = new List<Stats>();


    private List<Stats> endStats = new List<Stats>();


    [SerializeField]
    private TextMeshProUGUI[] serialNumberText;
    [SerializeField]
    private TextMeshProUGUI[] playerNamesTexts;
    [SerializeField]
    private TextMeshProUGUI[] killsTexts;
    [SerializeField]
    private TextMeshProUGUI[] deathsTexts;



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
        gameStats = new List<Stats>();
    }


    [System.Serializable]
    public struct Stats 
    {
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
    }


    public void AssignRanking()
    {

    }

















}
