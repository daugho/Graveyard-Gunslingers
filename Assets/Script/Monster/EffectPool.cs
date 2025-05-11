using System.Collections.Generic;
using System.Linq;
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
    public GameObject GetEffectExploEffect(string key)
    {
        if (!_effectPools.ContainsKey(key))
        {
            Debug.LogWarning($"[EffectPool] (Explosive) '{key}' 키를 찾을 수 없음");
            return null;
        }

        if (_effectPools[key].Count > 0)
        {
            var obj = _effectPools[key].Dequeue();
            return obj; // ? SetActive 하지 않음
        }
        else
        {
            Debug.LogWarning($"[EffectPool] (Explosive) '{key}' 풀에 남은 오브젝트가 없음");
            return null;
        }
    }
    public void ReturnEffect(string key, GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning($"[EffectPool] 반환하려는 '{key}' 이펙트가 이미 파괴됨(null)");
            return;
        }

        if (_effectPools.ContainsKey(key))
        {
            if (obj != null) obj.SetActive(false);
            _effectPools[key].Enqueue(obj);
        }
        else
        {
            //Debug.LogWarning($"[EffectPool] '{key}' 키가 풀에 없음. Destroy 수행");
            //if (obj != null) Destroy(obj);
        }
    }
}