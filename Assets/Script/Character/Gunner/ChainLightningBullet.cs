using UnityEngine;
using System.Collections.Generic;

public class ChainLightningBullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private LayerMask targetLayer;

    private float _damage;
    private int _maxBounce;
    private float _bounceRange;
    private Rigidbody _rb;

    private int _currentBounce = 0;
    private HashSet<GameObject> _hitTargets = new();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _currentBounce = 0;
        _hitTargets.Clear();
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void Initialize(float baseDamage, SkillData skillData)
    {
        _damage = baseDamage * skillData.Damage;
        _maxBounce = Mathf.FloorToInt(skillData.Rate);
        _bounceRange = skillData.Range;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hitTargets.Contains(other.gameObject)) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("DeadMonster")) return;

        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(_damage);
            _hitTargets.Add(other.gameObject);

            if (_currentBounce < _maxBounce)
            {
                _currentBounce++;
                GameObject nextTarget = FindClosestTarget(other.transform.position);
                if (nextTarget != null)
                {
                    transform.LookAt(nextTarget.transform);
                    _rb.linearVelocity = (nextTarget.transform.position - transform.position).normalized * 20f;
                    return;
                }
            }
        }

        ReturnToPool();
    }

    private GameObject FindClosestTarget(Vector3 from)
    {
        Collider[] colliders = Physics.OverlapSphere(from, _bounceRange, targetLayer);
        GameObject closest = null;
        float minDist = float.MaxValue;

        foreach (var col in colliders)
        {
            if (_hitTargets.Contains(col.gameObject)) continue;
            float dist = Vector3.Distance(from, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = col.gameObject;
            }
        }

        return closest;
    }

    private void ReturnToPool()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
