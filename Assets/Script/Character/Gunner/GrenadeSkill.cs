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
                Debug.Log($"[Grenade] 수류탄 +1 ▶ {_currentGrenadeCount}/{_maxGrenade}");
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
            Debug.Log("수류탄이 없습니다.");
            return;
        }

        _currentGrenadeCount--;
        _lastUseTime = Time.time;

        Gunner_grenade thrower = GetComponent<Gunner_grenade>();
        if (thrower != null)
        {
            thrower.ThrowWithCurrentVelocity();
        }

        Debug.Log($"[Grenade] 사용 ▶ 남은 수류탄: {_currentGrenadeCount}/{_maxGrenade}");
    }

    public void LevelUpSkill()
    {
        //SkillManager.Instance.AddOrLevelUpSkill("Grenade");
        SkillData data = SkillManager.Instance.GetPlayerSkill("Grenade");
        _maxGrenade = Mathf.FloorToInt(data.Rate);
        _currentGrenadeCount = _maxGrenade;
        _grenadeDamage = data.Damage;
        Debug.Log($"[Grenade] 스킬 해금 또는 레벨업 ▶ 소지 수류탄: {_currentGrenadeCount}/{_maxGrenade}");
    }
}
