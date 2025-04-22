using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public string SkillName;
    public float Cooldown;

    public abstract void Activate();
}
