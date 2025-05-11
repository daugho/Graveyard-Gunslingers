using System.Collections;
using UnityEngine;

public class ZombieRange : Monster
{
    private MonsterAnimatorController _anim;
    private bool _isAttacking = false;
    private bool _isAnimationPlaying = false;
    private GameObject _warningPrefab;

    [SerializeField] private LayerMask playerLayer;    // 타겟 레이어
    protected override void Start()
    {
        base.Start();
        _anim = GetComponent<MonsterAnimatorController>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            _target = playerObj.transform;
    }

    protected override void Update()
    {
        if (_isAnimationPlaying)
            return;
        if (_target == null || _stats == null)
            return;

        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= _attackRange && Time.time >= _lastAttackTime + _attackCooldown)
        {
            //_warningPrefab = EffectPool.Instance.GetEffect("Warning");
            AttackAnim();
        }
        else
        {
            Move();
        }
    }
    private void OnEnable()
    {
        ResetState();
    }
    private void ResetState()
    {
        // 스탯 초기화
        LoadMonsterStats(); // 체력 포함된 새로운 Stat 인스턴스 재설정
    }
    protected override void LoadMonsterStats()
    {
        _monsterType = MonsterType.Zombie;
        _monsterKey = 1002;

        MonsterData data = MonsterStatManager.Instance.GetStat(_monsterType, _monsterKey);
        if (data != null)
        {
            _stats = new StatManager.MonsterStats(data);
            _monsterName = data.Name;
            _attackRange = data.Range;

        }
    }
    protected override void Move()
    {
        if (_target == null)
            return;

        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= _attackRange)
        {
            AttackAnim();  // 공격 사거리 안에 들어오면 공격
            _navMeshAgent.isStopped = true;  // 이동 멈춤
        }
        else
        {
            Vector3 dir = (_target.position - transform.position).normalized; // 타겟 방향
            Vector3 destination = _target.position - dir * _attackRange;      // 사거리만큼 물러난 목적지

            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(destination);  // 사거리 유지 목적지로 이동
            _anim?.OnRun(); // 이동 애니메이션
        }
    }
    private void AttackAnim()
    {
        _isAnimationPlaying = true;
        _isAttacking = true;
        _lastAttackTime = Time.time;
        _anim?.OnAttack();
        SpawnAttackEffect();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
    private void SpawnAttackEffect()
    {

        GameObject warning = EffectPool.Instance.GetEffect(EffectKeys.Warning);
        Vector3 spawnPosition = _target.position;
        warning.transform.position = spawnPosition;
        warning.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        StartCoroutine(DelayedSkillSpawn(spawnPosition, warning));
    }
    private IEnumerator DelayedSkillSpawn(Vector3 position, GameObject warning)
    {
        yield return new WaitForSeconds(0.5f); // 경고 지속 시간

        // 경고 이펙트 반환
        if (warning != null)
            EffectPool.Instance.ReturnEffect(EffectKeys.Warning,warning);

        // 실제 공격 이펙트 소환
        _warningPrefab = EffectPool.Instance.GetEffect("ZombieRangeSkill");
        _warningPrefab.transform.position = position;
        _warningPrefab.transform.rotation = Quaternion.identity;

        ZombieRangeSkill skill = _warningPrefab.GetComponent<ZombieRangeSkill>();
        if (skill != null)
        {
            skill.Initialize(_stats.GetDamage(), _stats._range, playerLayer);
        }
    }
    protected override void OnDieAnimation()
    {
        _isAnimationPlaying = true;
        _anim.Ondie();
    }
    private void AnimEnd2()
    {
        _isAnimationPlaying = false;
        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= _attackRange && Time.time >= _lastAttackTime + _attackCooldown)
        {
            AttackAnim(); // 다시 공격
        }
        else
        {
            Move(); // 이동 계속
        }

    }
}