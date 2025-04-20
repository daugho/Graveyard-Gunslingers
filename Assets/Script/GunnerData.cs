using UnityEngine;

public class GunnerData : MonoBehaviour
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        Debug.Log($"[Gunner] Init 완료 → HP: {_hp}, Dmg: {_baseDmg}, Speed: {_speed}, AtkSpd: {_attackSpeed}");
    }
    public void LevelUp(int newLevel)
    {
        var stat = PlayerStatManager.Instance.GetStat(CharacterType.Gunner, newLevel);
        if (stat.Equals(default(PlayerStatData)))
        {
            Debug.LogError("레벨업 스탯 로드 실패!");
            return;
        }

        Initialize(stat); // 다시 초기화
        Debug.Log($"[Gunner] 레벨업 → {newLevel}레벨로 스탯 갱신");
    }
}
