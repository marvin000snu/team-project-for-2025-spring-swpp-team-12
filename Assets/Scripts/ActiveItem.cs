using UnityEngine;

[CreateAssetMenu(menuName = "Item/ActiveItem")]
public class ActiveItem : Item
{
    public int maxUses;

    public override void OnPickup(Player player)
    {
        player.inventory.SetActiveItem(this);
    }

    public override void Use(Player player)
    {
        if (maxUses <= 0) return;
        maxUses--;
        // 효과 발동 예: 폭탄 설치
        player.SpawnBomb();
        if (maxUses == 0)
            player.inventory.RemoveActiveItem();
    }
} 