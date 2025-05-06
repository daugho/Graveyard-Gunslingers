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

        // 폭발 이펙트 풀링
        GameObject explosion = EffectPool.Instance.GetEffect(EffectKeys.Explosion);
        if (explosion != null)
        {
            explosion.transform.position = transform.position;
            explosion.transform.rotation = Quaternion.identity;
            _explosionEffectObject = explosion;

            // 1.5초 후 Return
            Invoke(nameof(ReturnExplosionEffect), _explosionEffectLifetime);
        }

        // 카메라 흔들림
        Camera.main.GetComponent<QuarterViewCamera>()?.TriggerShake();

        // 데미지 및 폭발 처리
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, damageLayer);
        foreach (var hit in hitColliders)
        {
            if (hit.TryGetComponent<Monster>(out var monster))
                monster.TakeDamage(_damage);

            if (hit.attachedRigidbody != null)
                hit.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        // 수류탄을 풀로 반환 (SetActive(false))
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
