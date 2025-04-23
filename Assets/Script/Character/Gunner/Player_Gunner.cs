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
            Debug.LogError("스탯 로드 실패!");
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
            Debug.LogError("레벨업 스탯 로드 실패!");
            return;
        }

        _stats = newStats;
        //_currentLevel = _stats.Level;

        Debug.Log($"[Gunner] 레벨업 → {_stats.Level}레벨로 스탯 갱신 -> HP: {_stats.Health}, Dmg: {_stats.BaseDmg}, Speed: {_stats.Speed}, AtkSpd: {_stats.AttackSpeed}");

    }
}

