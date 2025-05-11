using UnityEngine;
using System.Collections;

public class ExplosionEffectSpawner : MonoBehaviour
{
    public static ExplosionEffectSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Spawn(string effectKey, Vector3 position, float duration = 1f)
    {
        GameObject effect = EffectPool.Instance.GetEffectExploEffect(effectKey); // ���� ���� ���
        if (effect == null) return;

        effect.transform.position = position;
        effect.transform.rotation = Quaternion.identity;
        effect.SetActive(true); // ���� ����

        StartCoroutine(ReturnAfterDelay(effectKey, effect, duration));
    }

    private IEnumerator ReturnAfterDelay(string key, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        EffectPool.Instance.ReturnEffect(key, obj);
    }
}
