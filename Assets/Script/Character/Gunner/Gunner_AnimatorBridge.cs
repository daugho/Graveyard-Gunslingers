using UnityEngine;

public class Gunner_AnimatorController : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void SetAim(bool isAiming)
    {
        _anim.SetBool("OnAim", isAiming);
    }

    public void SetSpeed(float speed)
    {
        _anim.SetFloat("Speed", speed);
    }

    public void SetMovementDirection(float h, float v)
    {
        _anim.SetFloat("Horizontal", h);
        _anim.SetFloat("Vertical", v);
    }

    public void TriggerReload()
    {
        _anim.SetLayerWeight(1, 1);
        Debug.Log("���� Ʈ���� ��");
    }
    public void OnReloadComplete()
    {
        _anim.SetLayerWeight(1, 0);
        Debug.Log("���� Ʈ���� ����");
    }
    // �ʿ信 ���� ����, �ǰ� �� �߰�
}
