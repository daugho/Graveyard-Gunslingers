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
            shooter.LoadSkillBullet(data); // 폭발탄 전용 Load 함수 필요 시 분리
        }

        Debug.Log($"[폭발탄] Lv.{data.Level} 사용! 반경 {data.Range} / 데미지 배수 {data.Damage}");
    }
}