using UnityEngine;

public class MonsterStatData
{
    private int _key;
    private string _name;
    private string _type;
    private float _hp;
    private float _moveSpeed;
    private float _damage;
    private float _range;
    private float _defense;
    private int _dropExp;
    private int _dropGold;
    private float _dropRate;
    private float _timeScaleFactor;
    private float _deadScaleFactor;
    private float _limitScale;
    private int _poolingScale;
    private int _poolingScaleMult;

    public int GetKey() => _key;
    public string GetName() => _name;
    public string GetTypeName() => _type;
    public float GetHp() => _hp;
    public float GetMoveSpeed() => _moveSpeed;
    public float GetDamage() => _damage;
    public float GetRange() => _range;
    public float GetDefense() => _defense;
    public int GetDropExp() => _dropExp;
    public int GetDropGold() => _dropGold;
    public float GetDropRate() => _dropRate;
    public float GetTimeScaleFactor() => _timeScaleFactor;
    public float GetDeadScaleFactor() => _deadScaleFactor;
    public float GetLimitScale() => _limitScale;
    public int GetPoolingScale() => _poolingScale;
    public int GetPoolingScaleMult() => _poolingScaleMult;


    public MonsterStatData(
        int key, string name, string type, float hp, float moveSpeed, float damage,
        float range, float defense, int dropExp, int dropGold, float dropRate,
        float timeScaleFactor, float deadScaleFactor, float limitScale,
        int poolingScale, int poolingScaleMult)
    {
        _key = key;
        _name = name;
        _type = type;
        _hp = hp;
        _moveSpeed = moveSpeed;
        _damage = damage;
        _range = range;
        _defense = defense;
        _dropExp = dropExp;
        _dropGold = dropGold;
        _dropRate = dropRate;
        _timeScaleFactor = timeScaleFactor;
        _deadScaleFactor = deadScaleFactor;
        _limitScale = limitScale;
        _poolingScale = poolingScale;
        _poolingScaleMult = poolingScaleMult;
    }
    private float GetDropMultiplier()
    {
        float decimalPart = _dropRate - Mathf.Floor(_dropRate); // ex: 2.011 ¡æ 0.011
        return 1f + decimalPart;
    }

    public int CalculateBonusExp()
    {
        return Mathf.RoundToInt(_dropExp * GetDropMultiplier());
    }

    public int CalculateBonusGold()
    {
        return Mathf.RoundToInt(_dropGold * GetDropMultiplier());
    }
}
