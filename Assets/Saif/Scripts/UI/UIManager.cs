using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : SimplePanel

{

    public GameObject FadeInScreen;
    public GameObject MainMenuPanel;
    public GameObject LoginPanel;
    public GameObject SelectionPanel;
    public GameObject CreateRoomPanel;
    public GameObject JoinRandomRoomPanel;
    public GameObject RoomlistPanel;
    public GameObject InsideRoomPanel;
    public GameObject LeaderboardPanel;


    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelTextGameplay;
    public TextMeshProUGUI CurrencyTextGameplay;
    public TextMeshProUGUI CurrencyTextMainMenu;


    public static UIManager instance;


    public GameObject startMultiplayerGameButton;


    public Image[] UIImages;



    public int totalScore;
    public int currentScore;


    public float updateDelay = 1.0f;


    int levelIndex;




    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else 
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        //GameManager.OnGameStart += OnClickPlay;
        //GameManager.OnGameWin += OnGameWin;
        //GameManager.OnGameLose += OnGameLose;
        //LevelManager.OnLevelLoad += OnLevelLoad;
        //ScoreManager.OnScoreChange += OnScoreChange;
    }


    private void OnDestroy()
    {
        //GameManager.OnGameStart -= OnClickPlay;
        //GameManager.OnGameWin -= OnGameWin;
        //GameManager.OnGameLose -= OnGameLose;
        //LevelManager.OnLevelLoad -= OnLevelLoad;
        //ScoreManager.OnScoreChange -= OnScoreChange;
    }


    // Start is called before the first frame update
    void Start()
    {
        CloseAndOpenPanel(null, MainMenuPanel);
    }

    //public void OnGameWin()
    //{
    //    //AudioManager.instance.PlayAudio(AudioType.SFX_UI_Success);
    //    StartCoroutine(UpdateLevelAndAddScore(true));
    //}
    //public void OnGameLose()
    //{
    //    //AudioManager.instance.PlayAudio(AudioType.SFX_UI_Failed);
    //    StartCoroutine(UpdateLevelAndAddScore(false));
    //}
  
  



    //private void OnLevelLoad(Level level, LevelManager.LevelTheme levelTheme)
    //{
    //    foreach (Image image in UIImages)
    //    {
    //        image.color = levelTheme.UIColor;
    //    }

    //    levelIndex = level.levelIndex;


    //    levelText.text = "Level " + level.levelIndex.ToString();
    //    levelTextGameplay.text = "Level " + level.levelIndex.ToString();


    //    CurrencyTextGameplay.text = PlayerPrefs.GetInt("TotalScore", 0).ToString();
    //    CurrencyTextMainMenu.text = PlayerPrefs.GetInt("TotalScore", 0).ToString();


    //}

    public void ShowGameWin()
    {
        
    }

    public void ShowGameLose()
    {

    }


    public void OnScoreChange(int score)
    {
        currentScore += score;
    }
    
    public void OnClickSinglePlayer()
    {
        CloseAndOpenPanel(SelectionPanel, null);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OnClickMultiplayer()
    {
        CloseAndOpenPanel(MainMenuPanel, LoginPanel);
    }










}
