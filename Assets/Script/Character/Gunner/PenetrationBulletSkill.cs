using UnityEngine;

public class PenetrationBulletSkill : MonoBehaviour
{
    private float _lastUseTime = -999f;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        TryActivateSkill();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        LevelUpSkill();
    //    }
    //}

    public void TryActivateSkill()
    {
        SkillData data = SkillManager.Instance.GetPlayerSkill("Pb");
        if (data == null)
        {
            return;
        }

        if (Time.time < _lastUseTime + data.Cooldown)
        {
            return;
        }

        _lastUseTime = Time.time;

        Gunner_Shoot shooter = GetComponent<Gunner_Shoot>();
        if (shooter != null)
        {
            shooter.LoadSkillBullet(data);
        }

        Debug.Log($"[°üÅëÅº] Lv.{data.Level} ¹ßµ¿! °üÅë·ü {data.Rate * 100f}%");
    }
}
