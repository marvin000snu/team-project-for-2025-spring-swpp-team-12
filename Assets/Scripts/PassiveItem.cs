using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Item/PassiveItem")]
public class PassiveItem : Item
{
    public float duration;

    public override void OnPickup(Player player)
    {
        player.StartCoroutine(ActivateEffect(player));
    }

    public override void Use(Player player) { /* 사용 안 함 */ }

    private IEnumerator ActivateEffect(Player player)
    {
        // 버프 적용
        player.moveSpeed *= 2;
        yield return new WaitForSeconds(duration);
        // 버프 해제
        player.moveSpeed /= 2;
    }
} 