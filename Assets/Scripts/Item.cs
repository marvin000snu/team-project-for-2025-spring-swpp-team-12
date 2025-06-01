using UnityEngine;

public enum ItemType {
    Passive,
    Active,
    Consumable
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    public abstract void OnPickup(Player player); // 먹었을 때
    public abstract void Use(Player player);      // 사용했을 때 (Passive는 즉시 발동, Active는 버튼 누를 때)
}