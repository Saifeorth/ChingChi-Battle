using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Spawner : MonoBehaviour
{


    public ChingChiCharacter player;
    public Transform playerSpawnPoint;
    public ChingchiAI enemy;
    public Transform[] enemySpawnPoints;
    public string[] enemyNames;

    public float respawnDelay;

    [SerializeField]
    private float MinArenaWidth;

    [SerializeField]
    private float MaxArenaWidth;

    [SerializeField]
    private float MinArenaDepth;

    [SerializeField]
    private float MaxArenaDepth;


    public static event Action<ChingChiCharacter> OnPlayeSpawned;

    public ScoreManager globalScoreManager;



    private void Start()
    {
        SpawnEnemies();
        SpawnPlayer();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }


    private void SpawnPlayer()
    {
        ChingChiCharacter chingchiPlayer = Instantiate(player, playerSpawnPoint.position, Quaternion.identity);
        chingchiPlayer.scoreManager = globalScoreManager;
        chingchiPlayer.SetName(PlayerPrefs.GetString("USERNAME"));
        globalScoreManager.AddStats(chingchiPlayer, chingchiPlayer.GetName());
        chingchiPlayer.gameObject.SetActive(true);
        OnPlayeSpawned?.Invoke(chingchiPlayer);
    }



    private void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            ChingchiAI chingchiEnemy = Instantiate(enemy, enemySpawnPoints[i].position, Quaternion.identity);
            chingchiEnemy.minArenaWidth = MinArenaWidth;
            chingchiEnemy.maxArenaWidth = MaxArenaWidth;
            chingchiEnemy.minArenaDepth = MinArenaDepth;
            chingchiEnemy.maxArenaDepth = MaxArenaDepth;
            chingchiEnemy.SetName(enemyNames[i]);
            chingchiEnemy.scoreManager = globalScoreManager;
            globalScoreManager.AddStats(chingchiEnemy, chingchiEnemy.GetName());
            chingchiEnemy.gameObject.SetActive(true);
        }
    }





    public void RespawnGameObject(ChingChiCharacter character)
    {
        character.Spawn();
    }




}
