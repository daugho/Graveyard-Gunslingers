using UnityEngine;

public class StatManager
{
    public class PlayerStats : IStat
    {
        public int _level { get; private set; }
        public float _baseDmg { get; private set; }
        public float _rateDmg { get; private set; }
        public float _luck { get; private set; }
        public float _dex { get; private set; }
        public float _health { get; private set; }
        public float _mp { get; private set; }
        public float _speed { get; private set; }
        public float _attackSpeed { get; private set; }
        public float _exp { get; private set; }

        public PlayerStats(PlayerStatData data)
        {
            _level = data.Level;
            _baseDmg = data.BaseDmg;
            _rateDmg = data.RateDmg;
            _luck = data.Luck;
            _dex = data.Dex;
            _health = data.Health;
            _mp = data.Mp;
            _speed = data.Speed;
            _attackSpeed = data.AttackSpeed;
            _exp = data.Exp;
        }

        public float GetHealth() => _health;
        public float GetMoveSpeed() => _speed;
        public float GetDamage() => _baseDmg * _rateDmg;
        public float GetDefense() => _dex * 0.5f; // ¿¹½Ã
    }

    public class MonsterStats : IStat
    {
        public float _health { get; private set; }
        public float _speed { get; private set; }
        public float _damage { get; private set; }
        public float _defense { get; private set; }

        public MonsterStats(MonsterData data)
        {
            _health = data.Hp;
            _speed = data.MoveSpeed;
            _damage = data.Damage;
            _defense = data.Defense;
        }

        public float GetHealth() => _health;
        public float GetMoveSpeed() => _speed;
        public float GetDamage() => _damage;
        public float GetDefense() => _defense;
    }
}
