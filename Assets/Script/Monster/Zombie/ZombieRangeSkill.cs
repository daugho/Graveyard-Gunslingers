using System.Collections;
using UnityEngine;

public class ZombieRangeSkill : MonoBehaviour
{
    private float _damage;
    private float _radius;
    private LayerMask _playerLayer;//플레이어인지 확인하기위한.

    public void Initialize(float damage, float radius, LayerMask playerLayer)
    {
        _damage = damage;
        _radius = 1.5f;
        _playerLayer = playerLayer;

        ActivateSkill();
    }

    private void ActivateSkill()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, _radius, _playerLayer);
        foreach (var player in hitPlayers)
        {
            IDamageable damageable = player.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
                Debug.Log($"[Zombie2Skill] 스킬 범위 내 플레이어에게 {_damage} 데미지 입힘!");
            }
        }
        StartCoroutine(ReturnToPool());
    }
    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(1.0f); // 이펙트 보이게 잠깐 유지
        EffectPool.Instance.ReturnEffect(EffectKeys.ZombieRangeSkill,gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}