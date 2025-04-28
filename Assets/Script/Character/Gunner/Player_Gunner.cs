using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Player_Gunner : MonoBehaviour
{
<<<<<<< Updated upstream
    private int _currentLevel = 1;
    private StatManager.PlayerStats _stats;
    public StatManager.PlayerStats Stats => _stats;
    private void Awake()
    {
=======
    private PlayerStats _stats;
    private TextMeshProUGUI _hpText;
    private TextMeshProUGUI _expText;
    private Slider _healthSlider;
    private Slider _expSlider;
    ////////////////////////////

    private int _curHp=0;
    private float _curExp=0;
    private float _maxHealth = 0;
    private float _finalDmg;
    private int _currentLevel = 1;


    private void Awake()
    {
        InitializeFromStatManager();
    }
    private void Start()
    {
        _maxHealth = _stats.Health;
        _curHp = (int)_maxHealth;

        _curExp = 0;

        _healthSlider = GameObject.Find("HpBar").GetComponent<Slider>();
        Transform hpTextTransform = _healthSlider.transform.GetChild(2); // ← 0부터 시작, 3번째 자식은 index 2
        _hpText = hpTextTransform.GetComponent<TextMeshProUGUI>();

        _expSlider = GameObject.Find("Expbar").GetComponent<Slider>();
        Transform expTextTransform = _expSlider.transform.GetChild(2); // ← 0부터 시작, 3번째 자식은 index 2
        _expText = expTextTransform.GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        
    }

    private void InitializeFromStatManager()
    {
>>>>>>> Stashed changes
        _stats = PlayerStatManager.Instance.GetPlayerStats(CharacterType.Gunner, _currentLevel);
        if (_stats == null)
        {
            Debug.LogError("스탯 로드 실패!");
        }
<<<<<<< Updated upstream
=======
        Debug.Log($"[Gunner] Init 완료 → HP: {_stats.Health}, Dmg: {_stats.BaseDmg}, Speed: {_stats.Speed}, AtkSpd: {_stats.AttackSpeed}");
>>>>>>> Stashed changes
    }
    public void LevelUp(int newLevel)
    {
        var newStats = PlayerStatManager.Instance.GetPlayerStats(CharacterType.Gunner, newLevel);
        if (newStats == null)
        {
            Debug.LogError("레벨업 스탯 로드 실패!");
            return;
        }

<<<<<<< Updated upstream
        _stats = newStats;
        //_currentLevel = _stats.Level;

        Debug.Log($"[Gunner] 레벨업 → {_stats._level}레벨로 스탯 갱신 -> HP: {_stats._health}, Dmg: {_stats._baseDmg}, Speed: {_stats._speed}, AtkSpd: {_stats._attackSpeed}");
        
=======
        _stats = newStats; // 스탯 갱신

        _currentLevel = _stats.Level;
        Debug.Log($"[Gunner] 레벨업 → {_currentLevel}레벨로 스탯 갱신");
    }
    public void GetExp(float exp)
    {
        _curExp += exp;
        if(_curExp>=_stats.Exp)
        {
            _currentLevel++;
            LevelUp(_currentLevel);
            _curExp %= _stats.Exp;
        }
        _expText.text = $"{(_curExp / _stats.Exp * 100f).ToString("F2")}%";
        _healthSlider.value = _curExp / _stats.Exp;
>>>>>>> Stashed changes
    }
    public void TakeDamage()
    {
        
    }
    //기본 스텟에 아이템으로 상승한 스텟을 계산한 최종 계산값을 이용하는 식으로.
    //아이템이 기본 스텟을 조정할수있음.
}

