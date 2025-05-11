using UnityEngine;

public class ExplosionEffectReturner : MonoBehaviour
{
    private string _effectKey;

    public void ScheduleReturn(string effectKey, float delay)
    {
        _effectKey = effectKey;
        Invoke(nameof(ReturnToPool), delay);
    }

    private void ReturnToPool()
    {
        if (EffectPool.Instance != null)
        {
            EffectPool.Instance.ReturnEffect(_effectKey, gameObject);
        }
        else
        {
            Debug.LogWarning($"[ExplosionEffectReturner] EffectPool�� �������� ����. ����Ʈ�� ��ȯ���� ����.");
        }
    }
}
