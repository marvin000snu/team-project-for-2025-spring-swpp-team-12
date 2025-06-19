using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData; 
    [SerializeField] private bool destroyOnPickup = true; // 아이템 획득 후 오브젝트 제거 여부

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && itemData != null)
        {
            Debug.Log($"[Item] {itemData.itemName} OnPickup called");
            itemData.OnPickup(other.gameObject);  // 아이템 효과 처리
            if (destroyOnPickup)
            {
                Destroy(gameObject);  // 아이템 오브젝트 제거
            }
        }
    }
}
