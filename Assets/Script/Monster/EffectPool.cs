using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    public static EffectPool Instance;

    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private int poolSize = 10;

    [SerializeField] private GameObject warningEffectPrefab;
    [SerializeField] private int warningPoolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private Queue<GameObject> warningPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(effectPrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
        for (int i = 0; i < warningPoolSize; i++)
        {
            GameObject obj = Instantiate(warningEffectPrefab, transform);
            obj.SetActive(false);
            warningPool.Enqueue(obj);
        }
    }

    public GameObject GetEffect()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(effectPrefab, transform);
            return obj;
        }
    }

    public void ReturnEffect(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
    public GameObject GetWarningEffect()
    {
        if (warningPool.Count > 0)
        {
            GameObject obj = warningPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(warningEffectPrefab, transform);
            return obj;
        }
    }

    public void ReturnWarningEffect(GameObject obj)
    {
        obj.SetActive(false);
        warningPool.Enqueue(obj);
    }
}