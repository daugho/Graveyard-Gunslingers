using UnityEngine;
using System.Collections.Generic;

public class PenetrationBullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;

    private float _damage;
    private float _pierceChance;
    private Rigidbody _rigidbody;

    private HashSet<Collider> _alreadyHit = new HashSet<Collider>();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _alreadyHit.Clear();
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
        _damage = baseDamage * skillData.Damage;
        _pierceChance = Mathf.Clamp01(skillData.Rate);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyHit.Contains(other))
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("DeadMonster"))
            return;

        _alreadyHit.Add(other);

        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(_damage);
        }

        if (Random.value >= _pierceChance)
        {
            ReturnToPool();
        }
        else
        {
        }
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
