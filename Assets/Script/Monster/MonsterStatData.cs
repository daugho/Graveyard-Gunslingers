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
    private int _ratekey;

    public int GetKey() => _key;
    public string GetName() => _name;
    public string GetTypeName() => _type;
    public float GetHp() => _hp;
    public float GetMoveSpeed() => _moveSpeed;
    public float GetDamage() => _damage;
    public float GetRange() => _range;
    public float GetDefense() => _defense;


}
