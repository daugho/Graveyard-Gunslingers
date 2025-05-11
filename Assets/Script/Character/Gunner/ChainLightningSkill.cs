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

        Debug.Log($"[체인라이트닝] Lv.{data.Level} 사용! 튕김 횟수 {data.Rate} / 범위 {data.Range} / 데미지 배수 {data.Damage}");
    }
}