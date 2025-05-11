using UnityEngine;

public class GrenadeSkill : MonoBehaviour
{
    private float _lastUseTime = -999f;
    private int _currentGrenadeCount;
    private float _cooldown;
    private int _maxGrenade;
    private float _regenTimer;
    private float _grenadeDamage;

    public float GetGrenadeDamage() => _grenadeDamage;
    private void Update()
    {
        SkillData data = SkillManager.Instance.GetPlayerSkill("Grenade");
        if (data == null) return;

        _cooldown = data.Cooldown;
        _maxGrenade = Mathf.FloorToInt(data.Rate);

        if (_currentGrenadeCount < _maxGrenade)
        {
            _regenTimer += Time.deltaTime;
            if (_regenTimer >= _cooldown)
            {
                _regenTimer = 0f;
                _currentGrenadeCount++;
                Debug.Log($"[Grenade] ����ź +1 �� {_currentGrenadeCount}/{_maxGrenade}");
            }
        }
    }
    public int GetCurrentCount()
    {
        return _currentGrenadeCount;
    }

    public void TryUseGrenade()
    {
        SkillData data = SkillManager.Instance.GetPlayerSkill("Grenade");
        if (data == null) return;

        if (_currentGrenadeCount <= 0)
        {
            Debug.Log("����ź�� �����ϴ�.");
            return;
        }

        _currentGrenadeCount--;
        _lastUseTime = Time.time;

        Gunner_grenade thrower = GetComponent<Gunner_grenade>();
        if (thrower != null)
        {
            thrower.ThrowWithCurrentVelocity();
        }

        Debug.Log($"[Grenade] ��� �� ���� ����ź: {_currentGrenadeCount}/{_maxGrenade}");
    }

    public void LevelUpSkill()
    {
        //SkillManager.Instance.AddOrLevelUpSkill("Grenade");
        SkillData data = SkillManager.Instance.GetPlayerSkill("Grenade");
        _maxGrenade = Mathf.FloorToInt(data.Rate);
        _currentGrenadeCount = _maxGrenade;
        _grenadeDamage = data.Damage;
        Debug.Log($"[Grenade] ��ų �ر� �Ǵ� ������ �� ���� ����ź: {_currentGrenadeCount}/{_maxGrenade}");
    }
}
