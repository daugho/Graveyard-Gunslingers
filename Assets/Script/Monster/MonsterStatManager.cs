using System.Collections.Generic;
using UnityEngine;
public class MonsterData
{
    public int Key;
    public string Name;
    public string Type;
    public float Hp;
    public float MoveSpeed;
    public float Damage;
    public float Range;
    public float Defense;
    public int RateKey;
}
public class MonsterStatManager : MonoBehaviour
{
    private static MonsterStatManager _instance;
    public static MonsterStatManager Instance => _instance ??= new MonsterStatManager();

    private Dictionary<MonsterType, Dictionary<int, MonsterData>> _monsterStats = new();

    private MonsterStatManager() { }

    public void LoadAllMonsterData()
    {
        foreach (MonsterType type in System.Enum.GetValues(typeof(MonsterType)))
        {
            LoadMonsterData(type);
        }

        Debug.Log("[MonsterStatManager] 모든 몬스터 데이터 로드 완료");
    }

    private void LoadMonsterData(MonsterType type)
    {
        string path = $"Data/MonsterData/{type.ToString().ToLower()}Data";  // 예: Resources/Data/MonsterData/ZombieData.csv
        TextAsset csv = Resources.Load<TextAsset>(path);

        if (csv == null)
        {
            Debug.LogError($"[MonsterStatManager] CSV 로드 실패: {path}");
            return;
        }

        Dictionary<int, MonsterData> statByKey = new();
        string[] lines = csv.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            MonsterData data = new MonsterData
            {
                Key = int.Parse(values[0]),
                Name = values[1],
                Type = values[2],
                Hp = float.Parse(values[3]),
                MoveSpeed = float.Parse(values[4]),
                Damage = float.Parse(values[5]),
                Range = float.Parse(values[6]),
                Defense = float.Parse(values[7]),
                RateKey = int.Parse(values[8])
            };

            statByKey[data.Key] = data;
        }

        _monsterStats[type] = statByKey;
        Debug.Log($"[MonsterStatManager] {type} 데이터 {statByKey.Count}개 로드 완료");
    }

    public MonsterData GetStat(MonsterType type, int key)
    {
        if (_monsterStats.TryGetValue(type, out var keyDict))
        {
            if (keyDict.TryGetValue(key, out var data))
                return data;
        }

        Debug.LogWarning($"[MonsterStatManager] {type}의 {key}레벨 데이터 없음");
        return null;
    }
}