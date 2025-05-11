using UnityEngine;

public class ChainLightningSkill : MonoBehaviour
{
    private float _lastUseTime = -999f;

    public void TryActivateSkill()
    {
        SkillData data = SkillManager.Instance.GetPlayerSkill("Lb");
        if (data == null || Time.time < _lastUseTime + data.Cooldown)
            return;

        _lastUseTime = Time.time;

        Gunner_Shoot shooter = GetComponent<Gunner_Shoot>();
        if (shooter != null)
        {
            shooter.LoadSkillBullet(data);
        }

        Debug.Log($"[ü�ζ���Ʈ��] Lv.{data.Level} ���! ƨ�� Ƚ�� {data.Rate} / ���� {data.Range} / ������ ��� {data.Damage}");
    }
}