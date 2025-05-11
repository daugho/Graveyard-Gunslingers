using UnityEngine;

public class GrenadeExplode : MonoBehaviour
{
    private float _damage;
    private bool _hasExploded = false;
    private GameObject _explosionEffectObject;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private float _explosionEffectLifetime = 1.0f;
    public void Initialize(float baseDamage, SkillData skillData)
    {
        _damage =  skillData.Damage;
    }
    public void SetDamage(float dmg)
    {
        _damage = dmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasExploded) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.CompareTag("Monster"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;

        // ���� ����Ʈ Ǯ��
        GameObject explosion = EffectPool.Instance.GetEffect(EffectKeys.Explosion);
        if (explosion != null)
        {
            explosion.transform.position = transform.position;
            explosion.transform.rotation = Quaternion.identity;
            _explosionEffectObject = explosion;

            // 1.5�� �� Return
            Invoke(nameof(ReturnExplosionEffect), _explosionEffectLifetime);
        }

        // ī�޶� ��鸲
        Camera.main.GetComponent<QuarterViewCamera>()?.TriggerShake();

        // ������ �� ���� ó��
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, damageLayer);
        foreach (var hit in hitColliders)
        {
            if (hit.TryGetComponent<Monster>(out var monster))
                monster.TakeDamage(_damage);
            Debug.Log($"������ : {_damage} ");

            //if (hit.attachedRigidbody != null)
            //    hit.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        // ����ź�� Ǯ�� ��ȯ (SetActive(false))
        GrenadePool.Instance.ReturnGrenade(gameObject);
    }

    private void ReturnExplosionEffect()
    {
        if (_explosionEffectObject != null)
        {
            EffectPool.Instance.ReturnEffect(EffectKeys.Explosion, _explosionEffectObject);
            _explosionEffectObject = null;
        }
    }
}
