using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnGameStart;
    public event Action OnGamePause;
    public event Action OnGameOver;
    public event Action OnGameClear;

    private bool isPaused = false;

    private float playTime = 0.0f;
    private string playerName = "";
    private int life = 3;



    [Serializable]
    public class HPChangedEvent : UnityEvent<int, int> { }
    public HPChangedEvent OnHPChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeHP(int curr, int max)
    {
        OnHPChanged?.Invoke(curr, max);
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        playTime = 0.0f;
        playerName = "";
        life = 3;
        OnGameStart?.Invoke();
    }

    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0;
        OnGamePause?.Invoke();
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1;
        OnGameStart?.Invoke();
    }

    public void GameOver()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0;
        OnGameOver?.Invoke();
    }

    public void GameClear()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0;
        OnGameClear?.Invoke();
    }

    public void SetPlayTime(float t)
    {
        playTime = t;
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }
    
    public float GetPlayTime() => playTime;
    public string GetPlayerName() => playerName;
}
