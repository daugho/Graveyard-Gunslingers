using UnityEngine;

public class ExplosiveBulletSkill : MonoBehaviour
{
    private float _lastUseTime = -999f;

    public void TryActivateSkill()
    {
        SkillData data = SkillManager.Instance.GetPlayerSkill("Eb");
        if (data == null || Time.time < _lastUseTime + data.Cooldown)
            return;

        _lastUseTime = Time.time;

        Gunner_Shoot shooter = GetComponent<Gunner_Shoot>();
        if (shooter != null)
        {
            shooter.LoadSkillBullet(data); // ����ź ���� Load �Լ� �ʿ� �� �и�
        }

        Debug.Log($"[����ź] Lv.{data.Level} ���! �ݰ� {data.Range} / ������ ��� {data.Damage}");
    }
}