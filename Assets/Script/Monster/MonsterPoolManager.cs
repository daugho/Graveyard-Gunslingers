using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour
{
    public static MonsterPoolManager Instance { get; private set; }

    [System.Serializable]
    public class MonsterPool
    {
        public MonsterType monsterType;
        public int monsterKey;
        public GameObject prefab;
        public int poolSize = 10;
    }

    [SerializeField]
    private List<MonsterPool> monsterPools;

    private Dictionary<(MonsterType, int), Queue<GameObject>> poolDictionary = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePools();
    }
    private float _spawnTimer = 0f;
    private float _spawnDelay = 5f; // 5초에 한 번

    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnDelay)
        {
            _spawnTimer = 0f;

            Vector3 spawnPos = GetRandomSpawnPosition();
            MonsterPoolManager.Instance.SpawnMonster(MonsterType.Zombie, 1001, spawnPos, Quaternion.identity);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0, z);
    }
    private void InitializePools()
    {
        foreach (var pool in monsterPools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add((pool.monsterType, pool.monsterKey), objectPool);
        }
    }

    public GameObject SpawnMonster(MonsterType type, int key, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey((type, key)))
        {
            Debug.LogWarning($"MonsterPoolManager: {type}-{key} 풀 없음");
            return null;
        }

        if (poolDictionary[(type, key)].Count == 0)
        {
            Debug.LogWarning($"MonsterPoolManager: {type}-{key} 풀 비어있음, 새로 생성");

            var prefab = monsterPools.Find(p => p.monsterType == type && p.monsterKey == key)?.prefab;
            if (prefab != null)
            {
                GameObject newObj = Instantiate(prefab);
                newObj.SetActive(false);
                poolDictionary[(type, key)].Enqueue(newObj);
            }
            else
            {
                Debug.LogError($"MonsterPoolManager: {type}-{key} 프리팹 없음! 풀 추가 실패");
            }
        }

        GameObject monster = poolDictionary[(type, key)].Dequeue();
        monster.SetActive(true);
        monster.transform.position = position;
        monster.transform.rotation = rotation;

        return monster;
    }

    public void ReturnMonster(MonsterType type, int key, GameObject monster)
    {
        monster.SetActive(false);
        poolDictionary[(type, key)].Enqueue(monster);
    }
}