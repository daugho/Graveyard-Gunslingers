using UnityEngine;
using Pathfinding;
public class Zombie1 : Monster
{
    MonsterAnimatorController _anim;
    private float _originalDetectRange =0f;
    protected bool _isAttacking = false;
    private bool _isAnimationPlaying = false;
    private float _attackSpeed = 2.0f;
    FollowerEntity ai;

    protected override void Start()
    {
        base.Start();
        _anim = GetComponent<MonsterAnimatorController>();
        _originalDetectRange = _detectRange;
    }
    protected override void Update()
    {
        if (_isAnimationPlaying)
            return;
        if (_target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                _target = playerObj.transform;
        }
        if (_target == null || _stats == null)
            return;
        float distance = Vector3.Distance(transform.position, _target.position);

        if (!_isChasing && distance <= _detectRange)
        {
            // 플레이어 감지 시작
            _isChasing = true;
            Debug.Log($"{_monsterName}이(가) 플레이어를 감지함");
        }

        if (_isChasing)
        {
            if (distance <= _attackRange)
            {
                AttackAnim();
            }
            else if (distance <= _detectRange)
            {
                Move();
            }
            else
            {
                _isChasing = false;
            }
        }
    }
    protected override void LoadMonsterStats()
    {
        _monsterType = MonsterType.Zombie;
        _monsterKey = 1001; // zombie1은 key 1001번
        MonsterData data = MonsterStatManager.Instance.GetStat(_monsterType, _monsterKey);

        if (data != null)
        {
            _stats = new StatManager.MonsterStats(data);
            _monsterName = data.Name;
            Debug.Log($"Zombie1:{_monsterName},{_stats._damage} , {_stats._speed} ");
        }
        else
        {
            Debug.LogError("Zombie1: 스탯 로드 실패");
        }
    }
    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= _attackRange)
        {
            AttackAnim();
            return;
        }
        else if (distance <= _detectRange)
        {
            base.Move();   // 이동 유지
            _anim?.OnRun(); // 이동 애니메이션
        }
        else
        {
            _isChasing = false;
            _detectRange = _originalDetectRange; // 감지 범위 복구
            _anim?.OnIdle();
            Debug.Log($"{_monsterName}: 플레이어 놓침, Idle 상태로 복귀");
        }
    }
    protected override void Attack()
    { 
        base.Attack();
    }
    private void AttackAnim()
    {
        _isAnimationPlaying = true;
        _isAttacking = true;
        _anim.OnAttack(); 
    }

    protected override void OnDieAnimation()
    {
        _isAnimationPlaying = true;
        _anim.Ondie();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _anim.OnHit();
        _isAnimationPlaying = true;
    }
    protected override void OnChaseStart()
    {
        _detectRange = _originalDetectRange * 1.5f; // Zombie1만 감지 범위 확장
        _anim.OnRun();
        Debug.Log("[Zombie1] 추적 시작, 감지 범위 확장!");
    }

    protected override void OnChaseEnd()
    {
        _detectRange = _originalDetectRange; // 감지 범위 복구
        _anim.OnIdle();
        Debug.Log("[Zombie1] 추적 종료, 감지 범위 복구");
    }
    private void AnimEnd()
    {
        _isAnimationPlaying = false;
        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= _attackRange)
        {
            AttackAnim(); // 다시 공격
        }
        else if (distance <= _detectRange)
        {
            Move(); // 이동 계속
        }
        else
        {
            _isChasing = false;
            _detectRange = _originalDetectRange;
            _anim?.OnIdle(); // Idle 대기
            Debug.Log($"{_monsterName}: 플레이어 놓쳐서 Idle로 전환");
        }
    }
}