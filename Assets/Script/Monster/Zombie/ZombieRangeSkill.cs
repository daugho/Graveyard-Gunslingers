using System.Collections;
using UnityEngine;

public class ZombieRangeSkill : MonoBehaviour
{
    private float _damage;
    private float _radius;
    private LayerMask _playerLayer;//�÷��̾����� Ȯ���ϱ�����.

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
                Debug.Log($"[Zombie2Skill] ��ų ���� �� �÷��̾�� {_damage} ������ ����!");
            }
        }
        StartCoroutine(ReturnToPool());
    }
    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(1.0f); // ����Ʈ ���̰� ��� ����
        EffectPool.Instance.ReturnEffect(EffectKeys.ZombieRangeSkill,gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}