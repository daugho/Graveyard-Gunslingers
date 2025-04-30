using UnityEngine;

public class Gunner_Move : MonoBehaviour
{
    float _playerbaseMoveSpeed = 6.0f;
    float _playerRotateSpeed = 12.0f;
    private bool _isAiming = false;
    public bool IsAiming => _isAiming;
    [SerializeField] private LayerMask groundLayer;
    private Player_Gunner _playerGunner;
    private Gunner_AnimatorController _animator;
    private void Awake()
    {
        _animator = GetComponent<Gunner_AnimatorController>();
        _playerGunner = GetComponent<Player_Gunner>();
    }
    void Update()
    {
        HandleMovement();
        HandleAiming();
        HandleRotation();
    }
    private void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _isAiming = true;
            _animator.SetAim(true);
            
            //_anim.SetLayerWeight(1, 1);
            //_anim.SetBool("IsAiming", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _isAiming = false;
            _animator.SetAim(false);

            //_anim.SetLayerWeight(1, 0);
            //_anim.SetBool("IsAiming", false);
        }
    }
    private void HandleMovement()
    {
        Vector3 inputRaw = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));//월드 좌표 d를 누르면 1,0,0 , 입력방향을 가져옴

        if (inputRaw.sqrMagnitude > 0.01f)
        {
            Vector3 moveDir = inputRaw.normalized;
            float speedMultiplier = _playerGunner.Stats._speed;
            float moveSpeed = _playerbaseMoveSpeed * speedMultiplier;
            float speed = (Input.GetKey(KeyCode.LeftShift) && (_isAiming==false)) ? moveSpeed * 3.0f : moveSpeed;
            transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

            Vector3 localDir = transform.InverseTransformDirection(moveDir);//월드 기준 방향(worldDir) → 이 오브젝트 기준의 방향(localDir)으로 바꿔줌
            _animator.SetMovementDirection(localDir.z, localDir.x);//localDir.x ->localDir.z로 변경. 성공.
            _animator.SetSpeed(speed);
        }
        else
        {
            _animator.SetMovementDirection(0, 0);
            _animator.SetSpeed(0);
        }
    }
    private void HandleRotation()
    {
        Vector3? lookDir = null;

        if (_isAiming)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                Vector3 targetPos = hit.point;
                targetPos.y = transform.position.y;
                lookDir = (targetPos - transform.position).normalized;
            }
        }
        else
        {
            Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (inputDir.sqrMagnitude > 0.01f)
            {
                lookDir = inputDir.normalized;
            }
        }

        if (lookDir.HasValue)
        {
            Quaternion lookRot = Quaternion.LookRotation(lookDir.Value);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, _playerRotateSpeed * Time.deltaTime);
        }
    }
}
