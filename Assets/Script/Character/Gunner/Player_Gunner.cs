using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Player_Gunner : MonoBehaviour
{
    private int _currentLevel = 1;
    private PlayerStats _stats;
    public PlayerStats Stats => _stats;
    private void Awake()
    {
        _stats = PlayerStatManager.Instance.GetPlayerStats(CharacterType.Gunner, _currentLevel);
        if (_stats == null)
        {
            Debug.LogError("���� �ε� ����!");
        }
    }

    //private void InitializeFromStatManager()
    //{
    //
    //}
    public void LevelUp(int newLevel)
    {
        var newStats = PlayerStatManager.Instance.GetPlayerStats(CharacterType.Gunner, newLevel);
        if (newStats == null)
        {
            Debug.LogError("������ ���� �ε� ����!");
            return;
        }

        _stats = newStats;
        //_currentLevel = _stats.Level;

        Debug.Log($"[Gunner] ������ �� {_stats.Level}������ ���� ���� -> HP: {_stats.Health}, Dmg: {_stats.BaseDmg}, Speed: {_stats.Speed}, AtkSpd: {_stats.AttackSpeed}");

    }
}

