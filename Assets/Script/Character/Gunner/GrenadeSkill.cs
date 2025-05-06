using UnityEngine;

public class GrenadeSkill : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float explosionForce = 5000f;
    [SerializeField] private LayerMask damageLayer;

    private bool _hasExploded = false;
    private float _damage = 100f;
    private GameObject _explosionEffectObject;
    private const float _explosionEffectLifetime = 1.0f;
    private void OnEnable()
    {
        _hasExploded = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Monster") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
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

            if (hit.attachedRigidbody != null)
                hit.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
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
