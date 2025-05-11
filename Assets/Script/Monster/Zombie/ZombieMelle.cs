using UnityEngine;
using Pathfinding;
using UnityEngine.AI;
using System.Collections;

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
    private bool _isHitProcessing = false;
    private float _attackSpeed = 2.0f;
    private NavMeshAgent _agent;
    private NavMeshObstacle _obstacle;
    [SerializeField] private Transform damageTextAnchor;
    protected override void Start()
    {
        base.Start();
        if (damageTextAnchor == null)
        {
            Transform found = transform.Find("HitTextPos");
            if (found != null)
            {
                damageTextAnchor = found;
            }
            else
            {
                Debug.LogWarning($"{name}�� HitTextPos�� �����ϴ�. �⺻ ��ġ�� ��µ˴ϴ�.");
            }
        }
        _anim = GetComponent<MonsterAnimatorController>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            _target = playerObj.transform;

        _agent = GetComponent<NavMeshAgent>();
        _obstacle = GetComponent<NavMeshObstacle>();

        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        _agent.avoidancePriority = Random.Range(30, 60);
        _obstacle.enabled = false;
    }
    private void OnEnable()
    {
        ResetState();
    }
    private void ResetState()
    {
        // ���� �ʱ�ȭ
        LoadMonsterStats(); // ü�� ���Ե� ���ο� Stat �ν��Ͻ� �缳��

        // ���� �ʱ�ȭ
        _state = ZombieState.Move;
        _isAnimationPlaying = false;
        _isHitProcessing = false;

        // NavMesh ���� �ʱ�ȭ
        if (_agent != null)
        {
            _agent.enabled = true;
            _agent.isStopped = false;
        }

        if (_obstacle != null)
            _obstacle.enabled = false;

        // �ִϸ��̼� �ʱ�ȭ
        _anim?.OnIdle(); // �ʱ� ��� ����

        // ��ġ NavMesh ����
        SnapToNavMeshSurface();
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
            Debug.LogError("ZombieMelle: ���� �ε� ����");
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
        Invoke(nameof(AttackEnd), _attackSpeed);
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
        if (_state == ZombieState.Die)
            return;
        Vector3 spawnPos = damageTextAnchor != null ? damageTextAnchor.position : transform.position + Vector3.up * 1.5f;

        DamageTextManager.Instance.Show(damage, spawnPos, DamageType.Normal);
        base.TakeDamage(damage);
        _anim?.OnHit();

        if (!_isHitProcessing)
        {
            _isHitProcessing = true;
            StartCoroutine(HandleHitStagger());
        }

        if (_stats.GetHealth() <= 0)
        {
            ChangeState(ZombieState.Die);
            OnDieAnimation();
        }
    }

    private IEnumerator HandleHitStagger()
    {
        _isAnimationPlaying = true;

        if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
            _agent.isStopped = true;

        _agent.enabled = false;
        _obstacle.enabled = true;

        yield return new WaitForSeconds(0.5f);

        // ? NavMesh ��ġ�� ����
        SnapToNavMeshSurface();

        _obstacle.enabled = false;
        _agent.enabled = true;

        yield return null;

        if (_agent.isOnNavMesh)
            _agent.isStopped = false;

        _isHitProcessing = false;
        _isAnimationPlaying = false;

        if (_state != ZombieState.Die)
            _state = ZombieState.Move;
    }

    /// <summary>
    /// ���� ��ġ ��ó���� ���� ����� NavMesh ���� ����
    /// </summary>
    private void SnapToNavMeshSurface()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
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


//using UnityEngine;
//using Pathfinding;
//using UnityEngine.AI;
//using System.Collections;

//public enum ZombieState
//{
//    Move,
//    Attack,
//    Die
//}
//public class ZombieMelle : Monster
//{
//    private ZombieState _state = ZombieState.Move;
//    private MonsterAnimatorController _anim;
//    private bool _isAnimationPlaying = false;
//    private bool _isHitProcessing = false;
//    private float _attackSpeed = 2.0f;
//    private NavMeshAgent _agent;
//    private NavMeshObstacle _obstacle;
//    protected override void Start()
//    {
//        base.Start();
//        _anim = GetComponent<MonsterAnimatorController>();
//        GameObject playerObj = GameObject.FindWithTag("Player");
//        if (playerObj != null)
//            _target = playerObj.transform;
//        _agent = GetComponent<NavMeshAgent>();
//        _obstacle = GetComponent<NavMeshObstacle>();
//        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
//        _agent.avoidancePriority = Random.Range(30, 60);
//        _obstacle.enabled = false;
//    }

//    protected override void Update()
//    {
//        if (_target == null || _state == ZombieState.Die)
//            return;

//        float distance = Vector3.Distance(transform.position, _target.position);

//        switch (_state)
//        {
//            case ZombieState.Move:
//                if (_isAnimationPlaying) return;

//                if (distance <= _attackRange)
//                    ChangeState(ZombieState.Attack);
//                else
//                    Move();
//                break;

//            case ZombieState.Attack:
//                if (!_isAnimationPlaying)
//                    AttackAnim();
//                break;
//        }
//    }

//    protected override void LoadMonsterStats()
//    {
//        _monsterType = MonsterType.Zombie;
//        _monsterKey = 1001;
//        var data = MonsterStatManager.Instance.GetStat(_monsterType, _monsterKey);

//        if (data != null)
//        {
//            _stats = new StatManager.MonsterStats(data);
//            _monsterName = data.Name;
//        }
//        else
//        {
//            Debug.LogError("ZombieMelle: ���� �ε� ����");
//        }
//    }

//    protected override void Move()
//    {
//        if (_target == null) return;
//        base.Move();
//        Vector3 dir = (_target.position - transform.position).normalized;
//        transform.LookAt(_target);
//        transform.Translate(dir * _stats.GetMoveSpeed() * Time.deltaTime, Space.World);

//        _anim?.OnRun();
//    }

//    private void AttackAnim()
//    {
//        _isAnimationPlaying = true;
//        _anim?.OnAttack();
//        Invoke(nameof(AttackEnd), _attackSpeed); // ���� �ִϸ��̼� �ð� �� ���� ���� ó��
//    }

//    private void AttackEnd()
//    {
//        _isAnimationPlaying = false;

//        if (_state != ZombieState.Die)
//        {
//            float distance = Vector3.Distance(transform.position, _target.position);
//            ChangeState(distance <= _attackRange ? ZombieState.Attack : ZombieState.Move);
//        }
//    }
//    public override void TakeDamage(float damage)
//    {
//        if (_state == ZombieState.Die)
//            return;

//        base.TakeDamage(damage);
//        _anim?.OnHit();

//        // �������� �׻� ������ ������Ʈ ��ȯ�� �ߺ� ����
//        if (!_isHitProcessing)
//        {
//            _isHitProcessing = true;
//            StartCoroutine(HandleHitStagger());
//        }

//        if (_stats.GetHealth() <= 0)
//        {
//            ChangeState(ZombieState.Die);
//            OnDieAnimation();
//        }
//    }
//    private IEnumerator HandleHitStagger()
//    {
//        _isAnimationPlaying = true;

//        // Agent �� Obstacle ��ȯ
//        if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
//            _agent.isStopped = true;

//        _agent.enabled = false;
//        _obstacle.enabled = true;

//        yield return new WaitForSeconds(0.5f); // ���� �ð�

//        // Obstacle �� Agent ����
//        _obstacle.enabled = false;
//        _agent.enabled = true;

//        yield return null; // �� ������ ��� �� NavMesh�� �ٰ� �ϰ�
//        if (_agent.isOnNavMesh)
//            _agent.isStopped = false;

//        _isHitProcessing = false;
//        _isAnimationPlaying = false;

//        if (_state != ZombieState.Die)
//            _state = ZombieState.Move;
//    }
//    // public override void TakeDamage(float damage)
//    // {
//    //     if (_state == ZombieState.Die) return;
//    //
//    //     base.TakeDamage(damage);
//    //     _anim?.OnHit();
//    //     _agent.isStopped = true;
//    //     _isAnimationPlaying = true;
//    //     _agent.enabled = false;
//    //     _obstacle.enabled = true;
//    //     Invoke(nameof(EndHitAnimation), 0.5f); // �ǰ� �ִϸ��̼� ���̸�ŭ ��ٸ� �� ����
//    //
//    //     if (_stats.GetHealth() <= 0)
//    //     {
//    //         ChangeState(ZombieState.Die);
//    //         OnDieAnimation();
//    //     }
//    // }

//    private void EndHitAnimation()
//    {
//        _isAnimationPlaying = false;
//        _agent.enabled = true;
//        _obstacle.enabled = false;
//        StartCoroutine(ResumeAgentNextFrame());
//        _state =ZombieState.Move;
//    }
//    private IEnumerator ResumeAgentNextFrame()
//    {
//        yield return null; // �� ������ ���
//        _agent.isStopped = false;
//    }

//    protected override void OnDieAnimation()
//    {
//        _isAnimationPlaying = true;
//        _anim?.Ondie();
//    }

//    private void ChangeState(ZombieState newState)
//    {
//        if (_state == newState) return;
//        _state = newState;
//    }
//}

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
//      //          // �ƹ��͵� �� ��
//      //          break;
//      //  }
//    }
//    protected override void LoadMonsterStats()
//    {
//        _monsterType = MonsterType.Zombie;
//        _monsterKey = 1001; // zombie1�� key 1001��
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
//            Debug.LogError("Zombie1: ���� �ε� ����");
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