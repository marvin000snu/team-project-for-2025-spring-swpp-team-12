using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

[DisallowMultipleComponent]
public class Stamina : MonoBehaviour
{
    [SerializeField] int maxStamina = 1000;
    [SerializeField] float recoveryRate = 10f; // 초당 회복량

    private int currentStamina;
    private bool isRecovering = true;
    private bool isStaminaLocked = false;

    public UnityEvent<int, int> OnStaminaChanged; // <current, max>

    public int CurrentStamina => currentStamina;
    public int MaxStamina => maxStamina;

    void Awake()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        // if (isRecovering && currentStamina < maxStamina)
        // {
        //     RecoverStamina();
        // }
        // Debug.Log("Current Stamina : " + currentStamina + "/" + maxStamina);
    }

    public bool IsStaminaAvailable()
    {
        if (currentStamina == 0 || isStaminaLocked)
        {
            return false;
        }
        return true;
    }
    public void ChangeStamina(int amount)
    {
        if (isStaminaLocked) return;
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        if (currentStamina == 0)
        {
            Debug.Log("Exhausted!");
            StartCoroutine(LockStamina(3f));
        }
    }

    private void RecoverStamina()
    {
        ChangeStamina(Mathf.RoundToInt(recoveryRate * Time.deltaTime));
    }


    private IEnumerator LockStamina(float duration)
    {
        isStaminaLocked = true;
        yield return new WaitForSeconds(duration);
        isStaminaLocked = false;
    }

    public void StopRecovery(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(StopRecoveryCoroutine(duration));
    }

    private IEnumerator StopRecoveryCoroutine(float duration)
    {
        isRecovering = false;
        yield return new WaitForSeconds(duration);
        isRecovering = true;
    }
}
