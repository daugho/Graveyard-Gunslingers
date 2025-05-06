using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    public static EffectPool Instance;

    [System.Serializable]
    public class EffectPoolItem
    {
        public string key;
        public GameObject prefab;
        public int poolSize = 10;
    }

    [SerializeField] private List<EffectPoolItem> effectPoolItems;
    private Dictionary<string, Queue<GameObject>> _effectPools = new();

    private void Awake()
    {
        Instance = this;

        foreach (var item in effectPoolItems)
        {
            Queue<GameObject> queue = new();
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            _effectPools[item.key] = queue;
        }
    }

    public GameObject GetEffect(string key)
    {
        if (!_effectPools.ContainsKey(key))
        {
            Debug.LogWarning($"[EffectPool] '{key}' 키를 찾을 수 없음");
            return null;
        }

        if (_effectPools[key].Count > 0)
        {
            var obj = _effectPools[key].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            Debug.LogWarning($"[EffectPool] '{key}' 풀에 남은 오브젝트가 없음");
            return null;
        }
    }

    public void ReturnEffect(string key, GameObject obj)
    {
        obj.SetActive(false);
        if (_effectPools.ContainsKey(key))
            _effectPools[key].Enqueue(obj);
        else
            Destroy(obj);
    }
}