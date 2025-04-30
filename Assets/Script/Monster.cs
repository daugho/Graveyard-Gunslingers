using UnityEngine;
using Pathfinding;
using UnityEngine.AI;
using System.Collections.Generic;
using System;
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
    protected float _attackCooldown = 3.0f;
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
        if (!gameObject.activeInHierarchy)
            return; // ��Ȱ��ȭ�Ǿ����� ���� ����

        if (_target == null)
            return;

        IDamageable damageable = _target.GetComponent<IDamageable>();
        damageable?.TakeDamage(_stats.GetDamage());
    }
    public virtual void TakeDamage(float damage)
    {
        _stats = new StatManager.MonsterStats(
            new MonsterData
            {
                Hp = _stats.GetHealth() - damage
            });
        if (_stats.GetHealth() <= 0)
        {
            OnDieAnimation();
        }
    }
    protected virtual void Die()
    {
        CancelInvoke();          // ��� Invoke ����
        StopAllCoroutines();     // ��� �ڷ�ƾ ����
        RoundManager.Instance.OnMonsterDie();
        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
        MonsterPoolManager.Instance.ReturnMonster(_monsterType, _monsterKey, gameObject);
        DropItem();
        DropExp();
    }
    protected virtual void OnDieAnimation()
    {
        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
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
    private void DropItem()
    {
        float dropChance = 0.2f;
        if (UnityEngine.Random.value > dropChance)
            return;

        string[] keys = { "Gold", "HpPotion", "MpPotion", "Box" };
        string selectedKey = keys[UnityEngine.Random.Range(0, keys.Length)];
        var monsterData = MonsterStatManager.Instance.GetStat(_monsterType, _monsterKey);
        ItemType type = Enum.Parse<ItemType>(selectedKey);
        var reward = DropTable.Instance.GetItem(monsterData.RateKey);
        if (reward == null)
        {
            Debug.LogError($"[DropItem] monsterKey({_monsterKey})�� �ش��ϴ� ��� �����Ͱ� �����ϴ�.");
            return;
        }
        ItemGrade grade = reward.Grade;

        GameObject prefab = ItemPrefabManager.Instance.GetItem(type, grade);
        if (prefab != null)
            Instantiate(prefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
    }
    private void DropExp()
    {
        var monsterData = MonsterStatManager.Instance.GetStat(_monsterType, _monsterKey);
        var reward = DropTable.Instance.GetItem(monsterData.RateKey);

        int exp = reward.Exp;
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            var hpExp = playerObj.GetComponent<Gunner_HpExp>();
            if (hpExp != null)
            {
                hpExp.GetExp(exp);
                Debug.Log($"[DropExp] ����ġ {exp} ���� �Ϸ�");
            }
        }
    }
}
