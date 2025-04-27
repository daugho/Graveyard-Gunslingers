using UnityEngine;

public class MonsterAnimatorController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnIdle()
    {
        if (_animator == null) return;
        _animator.SetFloat("State",0);
        
    }
    public void OnRun()
    {
        if (_animator == null) return;
        _animator.SetFloat("State",0.25f);
    }
    public void OnAttack()
    {
        if (_animator == null) return;
        _animator.SetFloat("State", 0.5f);
    }
    public void OnHit()
    {
        if (_animator == null) return;
        _animator.SetFloat("State", 0.75f);
    }
    public void Ondie()
    {
        _animator.SetBool("Dead", true);
        if (_animator == null) return;
    }

}
