public class SkillData
{
    public int Key;         // ��: 5041
    public string Name;
    public int Level;       // ���� ����
    public float Cooldown;
    public float Damage;
    public float Rate;
    public float Range;
    public bool IsUnlocked { get; private set; }

    public void Unlock()
    {
        IsUnlocked = true;
        Level = 1;
    }

    public void LevelUp()
    {
        Level++;
    }
}
