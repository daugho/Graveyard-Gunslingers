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
            Debug.LogWarning($"[EffectPool] '{key}' Ű�� ã�� �� ����");
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
            Debug.LogWarning($"[EffectPool] '{key}' Ǯ�� ���� ������Ʈ�� ����");
            return null;
        }
    }
    public GameObject GetEffectExploEffect(string key)
    {
        if (!_effectPools.ContainsKey(key))
        {
            Debug.LogWarning($"[EffectPool] (Explosive) '{key}' Ű�� ã�� �� ����");
            return null;
        }

        if (_effectPools[key].Count > 0)
        {
            var obj = _effectPools[key].Dequeue();
            return obj; // ? SetActive ���� ����
        }
        else
        {
            Debug.LogWarning($"[EffectPool] (Explosive) '{key}' Ǯ�� ���� ������Ʈ�� ����");
            return null;
        }
    }
    public void ReturnEffect(string key, GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning($"[EffectPool] ��ȯ�Ϸ��� '{key}' ����Ʈ�� �̹� �ı���(null)");
            return;
        }

        if (_effectPools.ContainsKey(key))
        {
            if (obj != null) obj.SetActive(false);
            _effectPools[key].Enqueue(obj);
        }
        else
        {
            //Debug.LogWarning($"[EffectPool] '{key}' Ű�� Ǯ�� ����. Destroy ����");
            //if (obj != null) Destroy(obj);
        }
    }
}