using System.Collections.Generic;
using UnityEngine;

public class ItemPrefabManager : MonoBehaviour
{
    public static ItemPrefabManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetItem(ItemType type, ItemGrade grade)
    {
        string path = $"Prefab/Item/{type}_{grade}";
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab == null)
        {
            Debug.LogWarning($"[ItemPrefabManager] Resources 경로에 프리팹 없음: {path}");
        }

        return prefab;
    }
}
