using UnityEngine;
using System.Collections;

public class TestGameClear : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(TriggerGameClearAfterDelay());
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