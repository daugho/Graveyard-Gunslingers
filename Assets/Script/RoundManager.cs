using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [System.Serializable]
    private class RoundSpawnData
    {
        public int round;
        public MonsterType type;
        public int key;
        public int count;
    }

    public static RoundManager Instance;

    [SerializeField] private string roundDataFile = "Data/SponData"; // Resources ���� ���
    private Dictionary<int, List<RoundSpawnData>> _roundData = new();
    private int _currentRound = 0;
    private int _aliveMonsterCount = 0;
    public GameObject nextRoundButton;

    private void Awake()
    {
        Instance = this;
        LoadRoundData();
    }

    private void LoadRoundData()
    {
        TextAsset csv = Resources.Load<TextAsset>(roundDataFile);
        if (csv == null)
        {
            Debug.LogError("RoundData CSV ������ �����ϴ�.");
            return;
        }

        string[] lines = csv.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] tokens = lines[i].Split(',');
            var data = new RoundSpawnData
            {
                round = int.Parse(tokens[0]),
                key = int.Parse(tokens[1]),
                type = (MonsterType)System.Enum.Parse(typeof(MonsterType), tokens[2]),
                count = int.Parse(tokens[3])
            };

            if (!_roundData.ContainsKey(data.round))
                _roundData[data.round] = new List<RoundSpawnData>();

            _roundData[data.round].Add(data);
        }

        Debug.Log("[RoundManager] ���� ������ �ε� �Ϸ�");

    }
    public Dictionary<(MonsterType, int), int> GetMaxSpawnCounts()
    {
        Dictionary<(MonsterType, int), int> maxCounts = new();

        foreach (var roundList in _roundData.Values)
        {
            foreach (var spawnData in roundList)
            {
                var key = (spawnData.type, spawnData.key);

                if (!maxCounts.ContainsKey(key))
                    maxCounts[key] = spawnData.count;
                else
                    maxCounts[key] = Mathf.Max(maxCounts[key], spawnData.count);
            }
        }

        return maxCounts;
    }
    public void StartNextRound()
    {
        _currentRound++;
        if (!_roundData.ContainsKey(_currentRound))
            {
            Debug.LogWarning($"[RoundManager] ���� {_currentRound} ���� �� StartNextRound �ߴ�");
            return;
        }
        nextRoundButton.SetActive(false);
        Invoke(nameof(SpawnCurrentRound), 1f);
    }

    private void SpawnCurrentRound()
    {
        foreach (var data in _roundData[_currentRound])
        {
            for (int i = 0; i < data.count; i++)
            {
                Vector3 spawnPos = GetRandomSpawnPosition();
                GameObject monster = MonsterPoolManager.Instance.SpawnMonster(data.type, data.key, spawnPos, Quaternion.identity);
                if (monster != null)
                    _aliveMonsterCount++;
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0, z);
    }

    public void OnMonsterDie()
    {
        _aliveMonsterCount--;
        if (_aliveMonsterCount <= 0)
        {
            Debug.Log($"[RoundManager] ���� {_currentRound} Ŭ����!");
            nextRoundButton.SetActive(true);
        }
    }
}