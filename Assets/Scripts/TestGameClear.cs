using UnityEngine;
using System.Collections;

public class TestGameClear : MonoBehaviour
{
    public PauseController pauseController;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        StartCoroutine(TriggerGameClearAfterDelay());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerMovement.SetCanMove(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseController.TogglePause();
        }
    }

    IEnumerator TriggerGameClearAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        if (GameManager.Instance != null)
        {
            Debug.Log("[TEST] 5초 경과: GameClear 호출");
            GameManager.Instance.GameClear();
        }
        else
        {
            Debug.LogWarning("GameManager.Instance가 존재하지 않습니다!");
        }
    }
}