using System.Collections;
using UnityEngine;

public class ZombieRange : Monster
{
    private MonsterAnimatorController _anim;
    private bool _isAttacking = false;
    private bool _isAnimationPlaying = false;
    private GameObject _warningPrefab;

    [SerializeField] private LayerMask playerLayer;    // Ÿ�� ���̾�
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
        // ���� �ʱ�ȭ
        LoadMonsterStats(); // ü�� ���Ե� ���ο� Stat �ν��Ͻ� �缳��
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
            AttackAnim();  // ���� ��Ÿ� �ȿ� ������ ����
            _navMeshAgent.isStopped = true;  // �̵� ����
        }
        else
        {
            Vector3 dir = (_target.position - transform.position).normalized; // Ÿ�� ����
            Vector3 destination = _target.position - dir * _attackRange;      // ��Ÿ���ŭ ������ ������

            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(destination);  // ��Ÿ� ���� �������� �̵�
            _anim?.OnRun(); // �̵� �ִϸ��̼�
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
        yield return new WaitForSeconds(0.5f); // ��� ���� �ð�

        // ��� ����Ʈ ��ȯ
        if (warning != null)
            EffectPool.Instance.ReturnEffect(EffectKeys.Warning,warning);

        // ���� ���� ����Ʈ ��ȯ
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
            AttackAnim(); // �ٽ� ����
        }
        else
        {
            Move(); // �̵� ���
        }

    }
}