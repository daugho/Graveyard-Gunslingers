using UnityEngine;

public abstract class StatBase
{
    public int Key { get; protected set; }
    public string Name { get; protected set; }
    public int Level { get; protected set; }

    public float BaseDmg { get; protected set; }
    public float RateDmg { get; protected set; }
    public float Health { get; protected set; }
    public float Speed { get; protected set; }

    // 필요시 Luck, Defense 등도 공통 처리
}
