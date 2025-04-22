using UnityEngine;

public abstract class RangedSkill : Skill
{
    public GameObject ProjectilePrefab;
    public float Speed;

    public override abstract void Activate();
}
