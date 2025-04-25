using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Player_Gunner : MonoBehaviour
{
    private int _currentLevel = 1;
    private StatManager.PlayerStats _stats; // �� ����� �κ�
    public StatManager.PlayerStats Stats => _stats;
    private void Awake()
    {
        _stats = PlayerStatManager.Instance.GetPlayerStats(CharacterType.Gunner, _currentLevel);
        if (_stats == null)
        {
            Debug.LogError("���� �ε� ����!");
        }
    }
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

        Debug.Log($"[Gunner] ������ �� {_stats._level}������ ���� ���� -> HP: {_stats._health}, Dmg: {_stats._baseDmg}, Speed: {_stats._speed}, AtkSpd: {_stats._attackSpeed}");

    }
}

