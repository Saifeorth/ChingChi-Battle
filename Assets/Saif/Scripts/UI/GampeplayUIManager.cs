using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class GampeplayUIManager : SimplePanel
{

    [SerializeField]
    private GameObject FadeInOutPanel;

    [SerializeField]
    private GameObject GamePlayPanel;

    [SerializeField]
    private GameObject GamePausedPanel;

    [SerializeField]
    private GameObject GameStatsPanel;


    [SerializeField]
    private TextMeshProUGUI gameTimeText;

    [SerializeField]
    private bool isGamePlaying;


    [SerializeField]
    private float gameTime = 300;


    public static event Action OnGamePause;
    public static event Action OnGameResume;



    private void Awake()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        gameTimeText.text = niceTime;
    }



    private void OnEnable()
    {
        Spawner.OnPlayeSpawned += OnPlayerSpawned;
        GameManager.OnGamePlay += OnGamePlay;
    }

    private void OnDisable()
    {
        Spawner.OnPlayeSpawned -= OnPlayerSpawned;
        GameManager.OnGamePlay -= OnGamePlay;
    }


    private void OnPlayerSpawned(ChingChiCharacter player)
    {
        CloseAndOpenPanel(null, FadeInOutPanel);
    }


    private void OnGamePlay(bool gamePlayingState)
    {
        isGamePlaying = gamePlayingState;
    }



    private void OnClickPause()
    {
        OnGamePause?.Invoke();
    }


    private void OnClickResume()
    {
        OnGameResume?.Invoke();
    }



    private void Update()
    {
        if (isGamePlaying)
        {
            gameTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(gameTime / 60f);
            int seconds = Mathf.FloorToInt(gameTime - minutes * 60);
            string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            gameTimeText.text = niceTime;
        }
    }





}
