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
    //[SerializeField] private Transform damageTextAnchor;

    protected Transform _target;
    protected bool _isChasing = false;
    protected float _detectRange = 8.0f;   // 플레이어를 감지하는 범위
    protected float _attackRange = 2.0f;   // 공격 사거리
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
        //if (damageTextAnchor == null)
        //{
        //    Transform found = transform.Find("HitTextPos");
        //    if (found != null)
        //    {
        //        damageTextAnchor = found;
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"{name}에 HitTextPos가 없습니다. 기본 위치로 출력됩니다.");
        //    }
        //}
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
        if (!gameObject.activeInHierarchy)
            return; // 비활성화되었으면 공격 무시

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
    //    Vector3 spawnPos = damageTextAnchor != null
    //? damageTextAnchor.position
    //: transform.position + Vector3.up * 1.5f;
    //
    //    DamageTextManager.Instance.Show(damage, spawnPos, DamageType.Normal);
        if (_stats.GetHealth() <= 0)
        {
            OnDieAnimation();
        }
    }
    protected virtual void Die()
    {
        CancelInvoke();          // 모든 Invoke 종료
        StopAllCoroutines();     // 모든 코루틴 종료
        RoundManager.Instance.OnMonsterDie();
        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
        DropItem();
        DropExp();
        MonsterPoolManager.Instance.ReturnMonster(_monsterType, _monsterKey, gameObject);

    }
    protected virtual void OnDieAnimation()
    {
        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
        // 자식 클래스(Zombie1 등)에서 오버라이드해서 애니메이션 재생 시작
    }
    private void DropItem()
    {
        float dropChance = 0.4f;
        if (UnityEngine.Random.value > dropChance)
            return;

        string[] keys = { "Gold", "HpPotion", "MpPotion", "Box" };
        string selectedKey = keys[UnityEngine.Random.Range(0, keys.Length)];
        var monsterData = MonsterStatManager.Instance.GetStat(_monsterType, _monsterKey);
        ItemType type = Enum.Parse<ItemType>(selectedKey);
        var reward = DropTable.Instance.GetItem(monsterData.RateKey);
        if (reward == null)
        {
            Debug.LogError($"[DropItem] monsterKey({_monsterKey})에 해당하는 드롭 데이터가 없습니다.");
            return;
        }
        ItemGrade grade = reward.Grade;

        GameObject prefab = ItemPrefabManager.Instance.GetItem(type, grade);
        if (prefab != null)
        {
            GameObject go = Instantiate(prefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            go.tag = "Item";
        }
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
                Debug.Log($"[DropExp] 경험치 {exp} 지급 완료");
            }
        }
    }
}
