using System.Collections.Generic;
using UnityEngine;

public class MonsterStatManager : MonoBehaviour
{
    private static MonsterStatManager _instance;
    public static MonsterStatManager Instance => _instance ??= new MonsterStatManager();

    private Dictionary<MonsterType, Dictionary<int, MonsterStatData>> _monsterStats = new();

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
        string path = $"Data/MonsterData/{type.ToString().ToLower()}";  // 예: Resources/Data/MonsterData/goblin.csv
        TextAsset csv = Resources.Load<TextAsset>(path);

        if (csv == null)
        {
            Debug.LogError($"[MonsterStatManager] CSV 로드 실패: {path}");
            return;
        }

        Dictionary<int, MonsterStatData> statByKey = new();
        string[] lines = csv.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            MonsterStatData data = new MonsterStatData
            (
             int.Parse(values[0]),  // key
             values[1],             // name
             values[2],             // type
             float.Parse(values[3]),// hp
             float.Parse(values[4]),// movespeed
             float.Parse(values[5]),// damage
             float.Parse(values[6]),// range
             float.Parse(values[7]),// defense
             int.Parse(values[8]),  // dropExp
             int.Parse(values[9]),  // dropGold
             float.Parse(values[10]),// dropRate
             float.Parse(values[11]),// TimeScaleFactor
             float.Parse(values[12]),// DeadScaleFactor
             float.Parse(values[13]),// LimitScale
             int.Parse(values[14]),  // poolingscale
             int.Parse(values[15])   // poolingscalemult
            );

            statByKey[data.GetKey()] = data;
        }

        _monsterStats[type] = statByKey;
        Debug.Log($"[MonsterStatManager] {type} 데이터 {statByKey.Count}개 로드 완료");
    }

    public MonsterStatData GetStat(MonsterType type, int key)
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