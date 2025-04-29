using UnityEngine;
using Pathfinding;
using UnityEngine.AI;
public abstract class Monster : MonoBehaviour, IDamageable
{
    protected StatManager.MonsterStats _stats;
    protected string _monsterName;
    protected int _monsterKey;
    protected MonsterType _monsterType;


    protected Transform _target;
    protected bool _isChasing = false;
    protected float _detectRange = 8.0f;   // �÷��̾ �����ϴ� ����
    protected float _attackRange = 2.0f;   // ���� ��Ÿ�
    protected float _attackCooldown = 2.0f;
    protected float _lastAttackTime = -999f;

    protected NavMeshAgent _navMeshAgent;


    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        LoadMonsterStats();
    }
    protected abstract void LoadMonsterStats();

    protected virtual void Update()
    {

    }

    protected virtual void Idle()
    {

    }
    protected virtual void Move()
    {
        _navMeshAgent.destination = _target.position;
        //transform.LookAt(_target);
        //transform.Translate(Vector3.forward * _stats.GetMoveSpeed() * Time.deltaTime);
    }
    protected virtual void Attack()
    {
        Debug.Log($"{_monsterName}��(��) �����մϴ�!");
        // Player�� Damageable �������̽��� �����ߴٰ� ����
        IDamageable damageable = _target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_stats.GetDamage());
        }
    }
    public virtual void TakeDamage(float damage)
    {
        _stats = new StatManager.MonsterStats(
            new MonsterData
            {
                Hp = _stats.GetHealth() - damage,
                MoveSpeed = _stats.GetMoveSpeed(),
                Damage = _stats.GetDamage(),
                Defense = _stats.GetDefense()
            });

        if (_stats.GetHealth() <= 0)
        {
            OnDieAnimation();
        }
    }
    protected virtual void Die()
    {
        transform.position = Vector3.zero;   // ���⼭ �ʱ�ȭ
        transform.localScale = Vector3.one;
        MonsterPoolManager.Instance.ReturnMonster(_monsterType, _monsterKey, gameObject);
    }
    protected virtual void OnDieAnimation()
    {
        // �ڽ� Ŭ����(Zombie1 ��)���� �������̵��ؼ� �ִϸ��̼� ��� ����
    }
    protected virtual void OnChaseStart()
    {
        // ���� ������ �� ���� (Run �ִϸ��̼����� ����)
    }

    protected virtual void OnChaseEnd()
    {
        // ���� ������ �� ���� (Idle �ִϸ��̼����� ����)
    }
}
