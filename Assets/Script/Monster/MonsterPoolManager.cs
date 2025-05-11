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
    private Dictionary<(MonsterType, int), GameObject> prefabDictionary = new();

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
    private void Start()
    {
        // ��� �ִ� ���� �������� Ǯ�� �� ���� �غ�
        PrewarmAllPools();
    }
    public void PrewarmAllPools()
    {
        var maxSpawnCounts = RoundManager.Instance.GetMaxSpawnCounts();

        foreach (var pair in maxSpawnCounts)
        {
            var (type, key) = pair.Key;
            int requiredCount = pair.Value;

            PreparePool(type, key, requiredCount);
        }

        Debug.Log("[MonsterPoolManager] ��� ���� Ǯ�� �Ϸ� (�ִ�ġ ����)");
    }
    private void InitializePools()
    {
        foreach (var pool in monsterPools)
        {
            var key = (pool.monsterType, pool.monsterKey);
            Queue<GameObject> objectPool = new();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary[key] = objectPool;
            prefabDictionary[key] = pool.prefab;
        }
    }

    public void PreparePool(MonsterType type, int key, int requiredCount)
    {
        var poolKey = (type, key);

        if (!poolDictionary.ContainsKey(poolKey) || !prefabDictionary.ContainsKey(poolKey))
        {
            Debug.LogError($"[MonsterPoolManager] {type}-{key}�� ���� Ǯ �Ǵ� ������ ����");
            return;
        }

        var queue = poolDictionary[poolKey];
        int lack = requiredCount - queue.Count;
        if (lack > 0)
        {
            GameObject prefab = prefabDictionary[poolKey];
            for (int i = 0; i < lack; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            Debug.Log($"[MonsterPoolManager] {type}-{key} ���� {lack}�� �߰� �غ� �Ϸ�");
        }
    }

    public GameObject SpawnMonster(MonsterType type, int key, Vector3 position, Quaternion rotation)
    {
        var poolKey = (type, key);

        if (!poolDictionary.ContainsKey(poolKey) || poolDictionary[poolKey].Count == 0)
        {
            Debug.LogWarning($"[MonsterPoolManager] {type}-{key} Ǯ ������� �� ���� ����");
            return null;
        }

        GameObject monster = poolDictionary[poolKey].Dequeue();
        monster.layer = LayerMask.NameToLayer("Monster");
        monster.transform.position = position;
        monster.transform.rotation = rotation;
        monster.SetActive(true);

        return monster;
    }

    public void ReturnMonster(MonsterType type, int key, GameObject monster)
    {
        monster.SetActive(false);
        monster.layer = LayerMask.NameToLayer("LivingMonster");
        poolDictionary[(type, key)].Enqueue(monster);
    }

    public int GetAvailableCount(MonsterType type, int key)
    {
        if (poolDictionary.TryGetValue((type, key), out var queue))
            return queue.Count;
        return 0;
    }
}

/*public class MonsterPoolManager : MonoBehaviour
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
    private float _spawnDelay = 5f; // 5�ʿ� �� ��

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
            Debug.LogWarning($"MonsterPoolManager: {type}-{key} Ǯ ����");
            return null;
        }

        if (poolDictionary[(type, key)].Count == 0)
        {
            Debug.LogWarning($"MonsterPoolManager: {type}-{key} Ǯ ������� �� ���� ����");
            return null;
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
}*/