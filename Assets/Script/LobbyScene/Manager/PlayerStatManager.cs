using System.Collections.Generic;
using System.IO;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerStatData
{
    public int Key;
    public string Name;
    public int Level;
    public float BaseDmg;
    public float RateDmg;
    public float Luck;
    public float Dex;
    public float Health;
    public float Mp;
    public float Speed;
    public float AttackSpeed;
    public float Exp;
}
public class PlayerStatManager : MonoBehaviour
{
    private static PlayerStatManager _instance;
    public static PlayerStatManager Instance => _instance ??= new PlayerStatManager();

    // 캐릭터별 레벨별 데이터
    private Dictionary<CharacterType, Dictionary<int, PlayerStatData>> _characterStats = new();

    private PlayerStatManager() { }

    public void LoadAllCharacterData()
    {
        foreach (CharacterType type in System.Enum.GetValues(typeof(CharacterType)))
        {
            LoadCharacterData(type);
        }

        Debug.Log($"[PlayerStatManager] 모든 캐릭터 데이터 로드 완료");
    }

    private void LoadCharacterData(CharacterType type)
    {
        string path = $"Data/PlayerData/{type.ToString().ToLower()}";
        Debug.Log($"[PlayerStatManager] CSV 로드 시도 경로: {path}");

        TextAsset csv = Resources.Load<TextAsset>(path);
        if (csv == null)
        {
            Debug.LogError($"CSV 파일 로드 실패: {path}");
            return;
        }

        Dictionary<int, PlayerStatData> statByLevel = new();
        string[] lines = csv.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            string[] values = line.Split(',');

            PlayerStatData data = new PlayerStatData
            {
                Key = int.Parse(values[0]),
                Name = values[1],
                Level = int.Parse(values[2]),
                BaseDmg = float.Parse(values[3]),
                RateDmg = float.Parse(values[4]),
                Luck = float.Parse(values[5]),
                Dex = float.Parse(values[6]),
                Health = float.Parse(values[7]),
                Mp = float.Parse(values[8]),
                Speed = float.Parse(values[9]),
                AttackSpeed = float.Parse(values[10]),
                Exp = float.Parse(values[11])
            };

            statByLevel[data.Level] = data;
        }

        _characterStats[type] = statByLevel;
        Debug.Log($"[PlayerStatManager] {type} 데이터 {statByLevel.Count}개 로드 완료");
    }

    public PlayerStatData GetStat(CharacterType type, int level)
    {
        if (_characterStats.TryGetValue(type, out var levelDict))
        {
            if (levelDict.TryGetValue(level, out var data))
                return data;
        }
        Debug.LogWarning($"[PlayerStatManager] {type}의 {level}레벨 데이터 없음");
        return default;
    }
    public StatManager.PlayerStats GetPlayerStats(CharacterType type, int level)
    {
        var data = GetStat(type, level);
        if (data.Equals(default(PlayerStatData)))
        {
            Debug.LogError($"[StatManager] {type} 레벨 {level} 데이터 없음!");
            return null;
        }

        return new StatManager.PlayerStats(data);
    }

}
