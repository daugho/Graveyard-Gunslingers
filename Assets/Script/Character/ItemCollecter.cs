using UnityEditor.ShaderGraph;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemCollector : MonoBehaviour
{
    private void Awake()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = 3f; // ������ ���� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        DroppedItem dropped = other.GetComponent<DroppedItem>();
        if (dropped != null)
        {
            Collect(dropped.itemType, dropped.itemGrade, dropped.rateKey);
            Destroy(other.gameObject);
        }
    }
    private void Collect(ItemType type, ItemGrade grade, int rateKey)
    {
        var dropData = DropTable.Instance.GetItem(rateKey);
        switch (type)
        {
            case ItemType.HpPotion:
                //HealPlayer(grade);
                break;
            case ItemType.Gold:
                Inventory.Instance?.AddGold(dropData.Gold);
                break;

            case ItemType.Box:
                Inventory.Instance?.AddBox(grade, dropData.Box);
                break;
            default:
                Debug.LogWarning($"[ItemCollector] ó������ ���� ������ Ÿ��: {type}");
                break;
        }
    }

}