using UnityEngine;

public class Zombie2Skill : MonoBehaviour
{
    private float _damage;
    private float _radius;
    private LayerMask _playerLayer;//�÷��̾����� Ȯ���ϱ�����.

    public void Initialize(float damage, float radius, LayerMask playerLayer)
    {
        _damage = damage;
        _radius = radius;
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
    }
}