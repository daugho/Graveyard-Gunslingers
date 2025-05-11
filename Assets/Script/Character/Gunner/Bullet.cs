using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;
    private float _damage;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeadMonster"))
            return;

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage); // 💥 여기서 몬스터가 텍스트 출력
        }

        ReturnToPool();
    }
    public void SetDamage(float damage) // ⭐️ 외부에서 데미지 설정
    {
        _damage = damage;
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
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}