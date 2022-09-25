using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public List<Stats> gameStats = new List<Stats>();



    public void UpdateScore(ChingChiCharacter whoKilled, ChingChiCharacter whoDied)
    {

    }


    [System.Serializable]
    public struct Stats 
    {
        public ChingChiCharacter character;
        public string characterName;
        public int Kills;
        public int Deaths;
    }




    public void AddStats(ChingChiCharacter player, string name)
    {
        Stats stats = new Stats();
        stats.character = player;
        stats.characterName = name;
        gameStats.Add(stats);
    }










}
