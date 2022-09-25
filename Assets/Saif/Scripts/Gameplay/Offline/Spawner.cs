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

    [SerializeField]
    private float MinArenaWidth;

    [SerializeField]
    private float MaxArenaWidth;

    [SerializeField]
    private float MinArenaDepth;

    [SerializeField]
    private float MaxArenaDepth;


    public static event Action<ChingChiCharacter> OnPlayeSpawned;



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
            chingchiEnemy.gameObject.SetActive(true);
        }
    }

}
