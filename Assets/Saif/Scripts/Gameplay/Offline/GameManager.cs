using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EZCameraShake;

public class GameManager : MonoBehaviour
{



    public int currentScore;
    public int RequiredScore;



    public static event Action<bool> OnGamePlay;



    public bool isGamePlaying = false;



    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        InGameAnnouncer.OnTimerAnnounced += StartGame;
        GampeplayUIManager.OnGamePause += OnGamePause;

        //LevelManager.OnLevelLoad += OnLevelLoad;
        //ScoreManager.OnScoreChange += OnScoreChange;
    }


    private void OnDisable()
    {
        InGameAnnouncer.OnTimerAnnounced -= StartGame;
        GampeplayUIManager.OnGamePause -= OnGamePause;
    }


    public void StartGame()
    {
        isGamePlaying = true;
        OnGamePlay?.Invoke(isGamePlaying);
    }



    private void OnGamePause()
    {
        isGamePlaying = false;
        OnGamePlay?.Invoke(isGamePlaying);
    }

    private void OnGameResume()
    {
        isGamePlaying = true;
        OnGamePlay?.Invoke(isGamePlaying);
    }







    //private void GameWin()
    //{
    //    OnGameWin?.Invoke();
    //}



    //private void GameLose()
    //{
    //    OnGameLose?.Invoke();
    //}


    //private void OnLevelLoad(Level level, LevelManager.LevelTheme levelTheme)
    //{
    //    RenderSettings.skybox = levelTheme.SkyboxMaterial;
    //    RenderSettings.fogColor = levelTheme.FogColor;


    //    RequiredScore = level.TargetHunger;
    //}


    //private void OnScoreChange(int score)
    //{
    //    currentScore += score;
    //    CheckGameWinLose(currentScore);
    //}



    //private void CheckGameWinLose(int currentScore)
    //{
    //    if (currentScore >= RequiredScore)
    //    {
    //        if (!isGameWin && !isGameLose)
    //        {
    //            isGameWin = true;
    //            PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + currentScore);
    //            PlayerPrefs.SetInt("LevelIndex", PlayerPrefs.GetInt("LevelIndex") + 1);
    //            OnGameWin?.Invoke();
    //        }

    //    }
    //    else if (currentScore <= 0)
    //    {
    //        if (!isGameLose && !isGameWin)
    //        {
    //            isGameLose = true;
    //            PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + currentScore);
    //            OnGameLose?.Invoke();
    //        }

    //    }
    //}


    private void OnDestroy()
    {
        //LevelManager.OnLevelLoad -= OnLevelLoad;
        //ScoreManager.OnScoreChange -= OnScoreChange;
    }







}
