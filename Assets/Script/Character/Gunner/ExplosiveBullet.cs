using UnityEngine;
using System.Collections.Generic;

public class ExplosiveBullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private LayerMask damageLayer;
    private float _damage;
    private Rigidbody _rigidbody;
     private float _baseExplosionRadius = 2f; // �⺻ ���� �ݰ�

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = transform.forward * 20f;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void Initialize(float baseDamage, SkillData skillData)
    {
        _damage = baseDamage + skillData.Damage;
        explosionRadius = _baseExplosionRadius * skillData.Range;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeadMonster"))
            return;
        Vector3 contactPoint = collision.contacts[0].point; // ? �浹 ����

        Explode(contactPoint);

    }
    private void Explode(Vector3 explosionPosition)
    {
        // 1. ���� ����Ʈ ���
        ExplosionEffectSpawner.Instance.Spawn(EffectKeys.Ebullet, explosionPosition);

        // 2. ������ ó��
        Collider[] hits = Physics.OverlapSphere(explosionPosition, explosionRadius, damageLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }

        // 3. ��ü Ǯ ��ȯ
        ReturnToPool();
    }


    private System.Collections.IEnumerator ReturnEffectAfterDelay(string key, GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        EffectPool.Instance.ReturnEffect(key, effect);
    }

    private void ReturnToPool()
    {
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
}
