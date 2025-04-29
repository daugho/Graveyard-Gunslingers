using UnityEngine;

public class Zombie2 : Monster
{
    private MonsterAnimatorController _anim;
    private bool _isAttacking = false;
    private bool _isAnimationPlaying = false;

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
            Debug.Log($"[Zombie2] ���� �ε� �Ϸ�: {_monsterName}, ������ {_stats.GetDamage()}, �̵��ӵ� {_stats.GetMoveSpeed()}");
        }
        else
        {
            Debug.LogError("[Zombie2] ���� �ε� ����");
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
            base.Move();   // �̵� ����
            _anim?.OnRun(); // �̵� �ִϸ��̼�
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
            AttackAnim(); // �ٽ� ����
        }
        else
        {
            Move(); // �̵� ���
        }

    }
}