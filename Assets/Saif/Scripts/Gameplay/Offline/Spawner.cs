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

    [SerializeField]
    private bool isGamePlaying = false;



    private void Start()
    {
        SpawnEnemies();
        SpawnPlayer();
    }

    private void OnEnable()
    {
        GameManager.OnGamePlay += OnGamePlay;
    }


    private void OnDisable()
    {
        GameManager.OnGamePlay -= OnGamePlay;
    }


    private void OnGamePlay(bool gamePlayingState)
    {
        isGamePlaying = gamePlayingState;
    }



    private void SpawnPlayer()
    {
        ChingChiCharacter chingchiPlayer = Instantiate(player, playerSpawnPoint.position, Quaternion.identity);
        chingchiPlayer.scoreManager = globalScoreManager;
        chingchiPlayer.SetName(PlayerPrefs.GetString("USERNAME"));
        chingchiPlayer.mySpawner = this;
        globalScoreManager.AddStats(chingchiPlayer);
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
            chingchiEnemy.mySpawner = this;
            chingchiEnemy.scoreManager = globalScoreManager;
            globalScoreManager.AddStats(chingchiEnemy);
            chingchiEnemy.gameObject.SetActive(true);
        }
    }



    public void RespawnGameObject(ChingChiCharacter character)
    {
        StartCoroutine(ShouldRespawnGameObject(character));
    }


    IEnumerator ShouldRespawnGameObject(ChingChiCharacter character)
    {
        character.transform.position =enemySpawnPoints[ UnityEngine.Random.Range(0, enemySpawnPoints.Length)].position;
        yield return new WaitForSeconds(respawnDelay);
        if (isGamePlaying)
        {
            character.isGamePlaying = isGamePlaying;
            character.gameObject.SetActive(true);
        }

    }


}
