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
    protected float _detectRange = 8.0f;   // 플레이어를 감지하는 범위
    protected float _attackRange = 2.0f;   // 공격 사거리
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
        Debug.Log($"{_monsterName}이(가) 공격합니다!");
        // Player가 Damageable 인터페이스를 구현했다고 가정
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
        transform.position = Vector3.zero;   // 여기서 초기화
        transform.localScale = Vector3.one;
        MonsterPoolManager.Instance.ReturnMonster(_monsterType, _monsterKey, gameObject);
    }
    protected virtual void OnDieAnimation()
    {
        // 자식 클래스(Zombie1 등)에서 오버라이드해서 애니메이션 재생 시작
    }
    protected virtual void OnChaseStart()
    {
        // 추적 시작할 때 실행 (Run 애니메이션으로 변경)
    }

    protected virtual void OnChaseEnd()
    {
        // 추적 종료할 때 실행 (Idle 애니메이션으로 변경)
    }
}
