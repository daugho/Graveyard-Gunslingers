using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    private Dictionary<string, List<SkillData>> _skillTable = new(); // name �� level ���
    private Dictionary<string, SkillData> _playerSkills = new();     // name �� ���� ����

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        LoadSkillData();
    }

    private void LoadSkillData()
    {
        TextAsset csv = Resources.Load<TextAsset>("Data/PlayerData/GunnerSkillData");
        if (csv == null)
        {
            Debug.LogError("[SkillManager] CSV ������ �����ϴ�.");
            return;
        }

        string[] lines = csv.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] tokens = lines[i].Split(',');
            string name = tokens[1];
            int level = int.Parse(tokens[2]);

            var data = new SkillData
            {
                Key = int.Parse(tokens[0]),
                Name = name,
                Level = level,
                Cooldown = float.Parse(tokens[3]),
                Damage = float.Parse(tokens[4]),
                Rate = float.Parse(tokens[5]),
                Range = float.Parse(tokens[6])
            };

            if (!_skillTable.ContainsKey(name))
                _skillTable[name] = new List<SkillData>();

            _skillTable[name].Add(data);
        }

        Debug.Log($"[SkillManager] ��ų {_skillTable.Count}���� ������ �ε� �Ϸ�");
    }

    public void AddOrLevelUpSkill(string name)
    {
        if (_playerSkills.TryGetValue(name, out var current))
        {
            var next = _skillTable[name].Find(s => s.Level == current.Level + 1);
            if (next != null)
            {
                _playerSkills[name] = next;
                Debug.Log($"[SkillManager] {name} ������ �� Lv.{next.Level},Damage: {next.Damage}");
                if (name == "Grenade")
                    SkillSlotManager.Instance.playerObject.GetComponent<GrenadeSkill>()?.LevelUpSkill();
            }
            else
            {
                Debug.Log($"[SkillManager] {name}�� �̹� �ִ� �����Դϴ�.");
            }
        }
        else
        {
            var first = _skillTable[name].Find(s => s.Level == 1);
            if (first != null)
            {
                _playerSkills[name] = first;
                Debug.Log($"[SkillManager] ��ų {name} �ر�!");

                Sprite icon = Resources.Load<Sprite>($"Image/{name}");
                SkillSlotManager.Instance.AddSkill(name, icon);
            }
        }
    }

    public SkillData GetPlayerSkill(string name)
    {
        return _playerSkills.TryGetValue(name, out var data) ? data : null;
    }
}
