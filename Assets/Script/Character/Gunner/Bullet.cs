using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;
    private float _damage;
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
        if (collision.gameObject.CompareTag("Monster"))
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            if (monster != null)
            {
                IDamageable damageable = monster as IDamageable;
                if (damageable != null)
                {
                    damageable.TakeDamage(_damage); // ⭐️ 몬스터 종류 상관없이 데미지 전달
                }
            }
        }

        ReturnToPool();
    }

    public void SetDamage(float damage) // ⭐️ 외부에서 데미지 설정
    {
        _damage = damage;
    }

    private void ReturnToPool()
    {
        BulletPool.Instance.ReturnBullet(gameObject);
    }
    //ridigbody
}