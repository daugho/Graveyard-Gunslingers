using UnityEngine;

public class PlayerStats
{
    public int Level;
    public float BaseDmg;
    public float RateDmg;
    public float Luck;
    public float Dex;
    public float Health;
    public float Mp;
    public float Speed;
    public float AttackSpeed;
    public float Exp;

    public PlayerStats(PlayerStatData data)
    {
        Level = data.Level;
        BaseDmg = data.BaseDmg;
        RateDmg = data.RateDmg;
        Luck = data.Luck;
        Dex = data.Dex;
        Health = data.Health;
        Mp = data.Mp;
        Speed = data.Speed;
        AttackSpeed = data.AttackSpeed;
        Exp = data.Exp;
    }
}
