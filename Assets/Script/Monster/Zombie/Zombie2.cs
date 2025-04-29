using UnityEngine;

public class Zombie2 : Monster
{
    private MonsterAnimatorController _anim;
    private bool _isAttacking = false;
    private bool _isAnimationPlaying = false;

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

        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= _attackRange)
        {
            AttackAnim();
        }
        else
        {
            Move();
        }
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
            Debug.Log($"[Zombie2] 스탯 로드 완료: {_monsterName}, 데미지 {_stats.GetDamage()}, 이동속도 {_stats.GetMoveSpeed()}");
        }
        else
        {
            Debug.LogError("[Zombie2] 스탯 로드 실패");
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
        else
        {
            base.Move();   // 이동 유지
            _anim?.OnRun(); // 이동 애니메이션
        }
    }

    private void AttackAnim()
    {
        _isAnimationPlaying = true;
        _isAttacking = true;
        _anim?.OnAttack();
        SpawnAttackEffect();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
    private void SpawnAttackEffect()
    {
        if (_target == null)
            return;

        GameObject skillObject = EffectPool.Instance.GetEffect();
        Vector3 spawnPosition = _target.position;
        skillObject.transform.position = spawnPosition;
        skillObject.transform.rotation = Quaternion.identity;

        Zombie2Skill skill = skillObject.GetComponent<Zombie2Skill>();
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

        if (distance <= _attackRange)
        {
            AttackAnim(); // 다시 공격
        }
        else
        {
            Move(); // 이동 계속
        }

    }
}