using UnityEngine;

[CreateAssetMenu(menuName = "Item/ConsumableItem")]
public class ConsumableItem : Item
{
    public int healAmount;

    public override void OnPickup(Player player)
    {
        Use(player); // 먹자마자 바로 사용
    }

    public override void Use(Player player)
    {
        player.Heal(healAmount);
    }
} 