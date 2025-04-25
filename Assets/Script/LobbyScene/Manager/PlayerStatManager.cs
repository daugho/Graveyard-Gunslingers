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

    // ĳ���ͺ� ������ ������
    private Dictionary<CharacterType, Dictionary<int, PlayerStatData>> _characterStats = new();

    private PlayerStatManager() { }

    public void LoadAllCharacterData()
    {
        foreach (CharacterType type in System.Enum.GetValues(typeof(CharacterType)))
        {
            LoadCharacterData(type);
        }

        Debug.Log($"[PlayerStatManager] ��� ĳ���� ������ �ε� �Ϸ�");
    }

    private void LoadCharacterData(CharacterType type)
    {
        string path = $"Data/PlayerData/{type.ToString().ToLower()}";
        Debug.Log($"[PlayerStatManager] CSV �ε� �õ� ���: {path}");

        TextAsset csv = Resources.Load<TextAsset>(path);
        if (csv == null)
        {
            Debug.LogError($"CSV ���� �ε� ����: {path}");
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
        Debug.Log($"[PlayerStatManager] {type} ������ {statByLevel.Count}�� �ε� �Ϸ�");
    }

    public PlayerStatData GetStat(CharacterType type, int level)
    {
        if (_characterStats.TryGetValue(type, out var levelDict))
        {
            if (levelDict.TryGetValue(level, out var data))
                return data;
        }
        Debug.LogWarning($"[PlayerStatManager] {type}�� {level}���� ������ ����");
        return default;
    }
    public StatManager.PlayerStats GetPlayerStats(CharacterType type, int level)
    {
        var data = GetStat(type, level);
        if (data.Equals(default(PlayerStatData)))
        {
            Debug.LogError($"[StatManager] {type} ���� {level} ������ ����!");
            return null;
        }

        return new StatManager.PlayerStats(data);
    }

}
