using UnityEngine;

public class Zombie2Skill : MonoBehaviour
{
    private float _damage;
    private float _radius;
    private LayerMask _playerLayer;//플레이어인지 확인하기위한.

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
                Debug.Log($"[Zombie2Skill] 스킬 범위 내 플레이어에게 {_damage} 데미지 입힘!");
            }
        }
    }
}