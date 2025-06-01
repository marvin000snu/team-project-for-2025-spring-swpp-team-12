using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Restore Health")]
public class RestoreHealthEffect : ScriptableObject, IEffect
{
    public int healAmount = 50;

    public void Apply(GameObject player)
    {
        Debug.Log($"[Effect] Restoring {healAmount} HP to {player.name}");
    }
}
