using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Player_Gunner : MonoBehaviour
{
    private int _level;
    private float _baseDmg;
    private float _rateDmg;
    private float _luck;
    private float _dex;
    private float _hp;
    private float _mp;
    private float _speed;
    private float _attackSpeed;
    private float _exp;

    ////////////////////////////

    private float _finalDmg;
    private float _curExp;
    private int _currentLevel = 0;

    private Slider _healthSlider;
    private TextMeshProUGUI _expText;
    private void Awake()
    {
        InitializeFromStatManager();
    }

    private void InitializeFromStatManager()
    {
        var statData = PlayerStatManager.Instance.GetStat(CharacterType.Gunner, _level);
        if (statData.Equals(default(PlayerStatData)))
        {
            Debug.LogError("[Player_Gunner] ���� �ε� ����!");
            return;
        }

        Initialize(statData);//ó�� �ҷ��ͼ� �ѹ� ����.
    }

    public void Initialize(PlayerStatData statData)
    {
        _level = statData.Level;
        _hp = statData.Health;
        _baseDmg = statData.BaseDmg;
        _rateDmg = statData.RateDmg;
        _speed = statData.Speed;
        _attackSpeed = statData.AttackSpeed;
        _luck = statData.Luck;
        _dex = statData.Dex;
        _mp = statData.Mp;
        _exp = statData.Exp;

        Debug.Log($"[Gunner] Init �Ϸ� �� HP: {_hp}, Dmg: {_baseDmg}, Speed: {_speed}, AtkSpd: {_attackSpeed}");
    }
    public void LevelUp(int newLevel)
    {
        var stat = PlayerStatManager.Instance.GetStat(CharacterType.Gunner, newLevel);
        if (stat.Equals(default(PlayerStatData)))
        {
            Debug.LogError("������ ���� �ε� ����!");
            return;
        }

        Initialize(stat); // �ٽ� �ʱ�ȭ
        Debug.Log($"[Gunner] ������ �� {newLevel}������ ���� ����");
    }
    public void GetExp(float exp)
    {
        _curExp += exp;
        if(_curExp>=_exp)
        {
            _currentLevel++;
            _curExp %= _exp;
        }
        _expText.text = $"{(_curExp / _exp * 100f).ToString("F2")}%";
        _healthSlider.value = _curExp / _exp;
    }
}
