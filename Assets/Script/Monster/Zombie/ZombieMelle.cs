using UnityEngine;
using Pathfinding;
using UnityEngine.AI;

public enum ZombieState
{
    Move,
    Attack,
    Die
}
public class ZombieMelle : Monster
{
    private ZombieState _state = ZombieState.Move;
    private MonsterAnimatorController _anim;
    private bool _isAnimationPlaying = false;
    private float _attackSpeed = 2.0f;
    private NavMeshAgent _agent;
    protected override void Start()
    {
        base.Start();
        _anim = GetComponent<MonsterAnimatorController>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            _target = playerObj.transform;
        _agent = GetComponent<NavMeshAgent>();

        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        _agent.avoidancePriority = Random.Range(30, 60);
    }

    protected override void Update()
    {
        if (_target == null || _state == ZombieState.Die)
            return;

        float distance = Vector3.Distance(transform.position, _target.position);

        switch (_state)
        {
            case ZombieState.Move:
                if (_isAnimationPlaying) return;

                if (distance <= _attackRange)
                    ChangeState(ZombieState.Attack);
                else
                    Move();
                break;

            case ZombieState.Attack:
                if (!_isAnimationPlaying)
                    AttackAnim();
                break;
        }
    }

    protected override void LoadMonsterStats()
    {
        _monsterType = MonsterType.Zombie;
        _monsterKey = 1001;
        var data = MonsterStatManager.Instance.GetStat(_monsterType, _monsterKey);

        if (data != null)
        {
            _stats = new StatManager.MonsterStats(data);
            _monsterName = data.Name;
        }
        else
        {
            Debug.LogError("ZombieMelle: 스탯 로드 실패");
        }
    }

    protected override void Move()
    {
        if (_target == null) return;
        base.Move();
        Vector3 dir = (_target.position - transform.position).normalized;
        transform.LookAt(_target);
        transform.Translate(dir * _stats.GetMoveSpeed() * Time.deltaTime, Space.World);

        _anim?.OnRun();
    }

    private void AttackAnim()
    {
        _isAnimationPlaying = true;
        _anim?.OnAttack();
        Invoke(nameof(AttackEnd), _attackSpeed); // 공격 애니메이션 시간 후 공격 종료 처리
    }

    private void AttackEnd()
    {
        _isAnimationPlaying = false;

        if (_state != ZombieState.Die)
        {
            float distance = Vector3.Distance(transform.position, _target.position);
            ChangeState(distance <= _attackRange ? ZombieState.Attack : ZombieState.Move);
        }
    }

    public override void TakeDamage(float damage)
    {
        if (_state == ZombieState.Die) return;

        base.TakeDamage(damage);
        _anim?.OnHit();

        _isAnimationPlaying = true;
        Invoke(nameof(EndHitAnimation), 0.5f); // 피격 애니메이션 길이만큼 기다린 뒤 해제

        if (_stats.GetHealth() <= 0)
        {
            ChangeState(ZombieState.Die);
            OnDieAnimation();
        }
    }

    private void EndHitAnimation()
    {
        _isAnimationPlaying = false;
        _state=ZombieState.Move;
    }

    protected override void OnDieAnimation()
    {
        _isAnimationPlaying = true;
        _anim?.Ondie();
    }

    private void ChangeState(ZombieState newState)
    {
        if (_state == newState) return;
        _state = newState;
    }
}

//enum ZombieState
//{
//    Move,
//    Attack,
//    Hit,
//    Die
//}
//public class ZombieMelle : Monster
//{
//    private ZombieState _state = ZombieState.Move;
//    MonsterAnimatorController _anim;
//    private float _originalDetectRange =0f;
//    protected bool _isAttacking = false;
//    private bool _isAnimationPlaying = false;
//    private float _attackSpeed = 2.0f;
//    FollowerEntity ai;
//
//    protected override void Start()
//    {
//        base.Start();
//        _anim = GetComponent<MonsterAnimatorController>();
//        _originalDetectRange = _detectRange;
//        GameObject playerObj = GameObject.FindWithTag("Player");
//        if (playerObj != null)
//            _target = playerObj.transform;
//    }
//    protected override void Update()
//    {
//
//      //  if (_isAnimationPlaying)
//      //      return;
//      //
//      //  if (_target == null)
//      //      return;
//      //
//      //  float distance = Vector3.Distance(transform.position, _target.position);
//      //  switch (_state)
//      //  {
//      //      case ZombieState.Move:
//      //          if (Vector3.Distance(transform.position, _target.position) <= _attackRange)
//      //          {
//      //              _state = ZombieState.Attack;
//      //          }
//      //          else
//      //          {
//      //              Move();
//      //          }
//      //          break;
//      //
//      //      case ZombieState.Attack:
//      //          AttackAnim(); break;
//      //
//      //      case ZombieState.Die:
//      //          // 아무것도 안 함
//      //          break;
//      //  }
//    }
//    protected override void LoadMonsterStats()
//    {
//        _monsterType = MonsterType.Zombie;
//        _monsterKey = 1001; // zombie1은 key 1001번
//        MonsterData data = MonsterStatManager.Instance.GetStat(_monsterType, _monsterKey);
//
//        if (data != null)
//        {
//            _stats = new StatManager.MonsterStats(data);
//            _monsterName = data.Name;
//            Debug.Log($"Zombie1:{_monsterName},{_stats._damage} , {_stats._speed} ,{_stats._health},{_stats._defense}");
//        }
//        else
//        {
//            Debug.LogError("Zombie1: 스탯 로드 실패");
//        }
//    }
//    protected override void Move()
//    {
//        base.Move();
//        float distance = Vector3.Distance(transform.position, _target.position);
//
//        if (_target == null) return;
//
//        Vector3 dir = (_target.position - transform.position).normalized;
//        transform.LookAt(_target);
//        transform.Translate(dir * _stats.GetMoveSpeed() * Time.deltaTime, Space.World);
//
//        _anim?.OnRun();
//    }
//    private void AttackAnim()
//    {
//        _isAnimationPlaying = true;
//        _isAttacking = true;
//        _anim.OnAttack(); 
//    }
//
//    protected override void OnDieAnimation()
//    {
//        _isAnimationPlaying = true;
//        _anim.Ondie();
//    }
//
//    public override void TakeDamage(float damage)
//    {
//        base.TakeDamage(damage);
//        _anim.OnHit();
//        _isAnimationPlaying = true;
//    }
//    private void AnimEnd()
//    {
//        _isAnimationPlaying = false;
//        float distance = Vector3.Distance(transform.position, _target.position);
//
//        if (_stats.GetHealth() <= 0)
//        {
//            _state = ZombieState.Die;
//            return;
//        }
//
//        if (distance <= _attackRange)
//        {
//            _state = ZombieState.Attack;
//        }
//        else
//        {
//            _state = ZombieState.Move;
//        }
//
//    }
//}