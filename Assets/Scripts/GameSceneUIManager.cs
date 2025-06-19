using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSceneUIManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject clearPanel;
    public GameObject overPanel;

    public PlayerMovement playerMovement;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePause += PauseGame;
            GameManager.Instance.OnGameStart += ResumeGame;
            GameManager.Instance.OnGameClear += GameClear; // 게임 클리어 후 UI 초기화
            GameManager.Instance.OnGameOver += GameOver;  // 게임 오버 후 UI 초기화
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePause -= PauseGame;
            GameManager.Instance.OnGameStart -= ResumeGame;
            GameManager.Instance.OnGameClear -= GameClear;
            GameManager.Instance.OnGameOver -= GameOver;
        }
    }

    public void PauseGame()
    {
        //pause 되면 패널 켜기
        pausePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerMovement.SetCanMove(false);
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerMovement.SetCanMove(true);
    }

    public void GameClear()
    {
        clearPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerMovement.SetCanMove(false);
    }

    public void GameOver()
    {
        overPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerMovement.SetCanMove(false);
    }
}
