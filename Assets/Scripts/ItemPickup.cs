using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && itemData != null)
        {
            Debug.Log($"[Item] {itemData.itemName} OnPickup called");
            itemData.OnPickup(other.gameObject);  // 아이템 효과 처리
            Destroy(gameObject);                  // ✅ 무조건 아이템 오브젝트 제거
        }
    }
}
