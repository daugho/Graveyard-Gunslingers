using UnityEngine;

public abstract class Player : MonoBehaviour
{
    protected Animator _anim;
    protected float _playerMoveSpeed = 2f;
    protected bool _isRunning = false;
    protected float _playerRotateSpeed = 12.0f;

    protected virtual void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    protected virtual void Update()
    {
        HandleMovement();
    }
    protected virtual void HandleMovement()
    {
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (inputDir.sqrMagnitude > 0.01f)
        {
            transform.Translate(inputDir.normalized * _playerMoveSpeed * Time.deltaTime, Space.World);
        }

        _anim.SetFloat("Speed", inputDir.magnitude);
    }
    public abstract void Attack();

}
