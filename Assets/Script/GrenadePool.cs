using System.Collections.Generic;
using UnityEngine;

public class GrenadePool : MonoBehaviour
{
    public static GrenadePool Instance;

    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pool = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(grenadePrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetGrenade()
    {
        GameObject grenade = pool.Count > 0 ? pool.Dequeue() : Instantiate(grenadePrefab);
        grenade.SetActive(true);
        return grenade;
    }

    public void ReturnGrenade(GameObject grenade)
    {
        grenade.SetActive(false);
        pool.Enqueue(grenade);
    }
}
