using UnityEngine;
using Pathfinding;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
        if (distance <= _attackRange)
        {
            AttackAnim(); // ��Ÿ� ���� ������ ����
        }
        else
        {
            Move(); // �׻� ����
        }
    }
    protected override void LoadMonsterStats()
    {
        _monsterType = MonsterType.Zombie;
        _monsterKey = 1001; // zombie1�� key 1001��
        MonsterData data = MonsterStatManager.Instance.GetStat(_monsterType, _monsterKey);

        if (data != null)
        {
            _stats = new StatManager.MonsterStats(data);
            _monsterName = data.Name;
            Debug.Log($"Zombie1:{_monsterName},{_stats._damage} , {_stats._speed} ,{_stats._health},{_stats._defense}");
        }
        else
        {
            Debug.LogError("Zombie1: ���� �ε� ����");
        }
    }
    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, _target.position);

        if (_target == null) return;

        Vector3 dir = (_target.position - transform.position).normalized;
        transform.LookAt(_target);
        transform.Translate(dir * _stats.GetMoveSpeed() * Time.deltaTime, Space.World);

        _anim?.OnRun();
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
        _detectRange = _originalDetectRange * 1.5f; // Zombie1�� ���� ���� Ȯ��
        _anim.OnRun();
        Debug.Log("[Zombie1] ���� ����, ���� ���� Ȯ��!");
    }

    protected override void OnChaseEnd()
    {
        _detectRange = _originalDetectRange; // ���� ���� ����
        _anim.OnIdle();
        Debug.Log("[Zombie1] ���� ����, ���� ���� ����");
    }
    private void AnimEnd()
    {
        _isAnimationPlaying = false;
        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= _attackRange)
        {
            AttackAnim(); // ��Ÿ� ���� ������ ����
        }
        else
        {
            Move(); // �׻� ����
        }
    }
}