using UnityEngine;

public class Gunner_Move : MonoBehaviour
{
    Animator _anim;
    float _playerMoveSpeed = 2.0f;
    float _playerRotateSpeed = 12.0f;
    private bool _isAiming = false;
    public bool IsAiming => _isAiming;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
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
            _anim.SetBool("OnAim", true);
            
            //_anim.SetLayerWeight(1, 1);
            //_anim.SetBool("IsAiming", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _isAiming = false;
            _anim.SetBool("OnAim", false);

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

            float speed = Input.GetKey(KeyCode.LeftShift) ? _playerMoveSpeed * 3.0f : _playerMoveSpeed;
            transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

            Vector3 localDir = transform.InverseTransformDirection(moveDir);//월드 기준 방향(worldDir) → 이 오브젝트 기준의 방향(localDir)으로 바꿔줌
            _anim.SetFloat("Horizontal", localDir.z);//localDir.x ->localDir.z로 변경. 성공.
            _anim.SetFloat("Vertical", localDir.x);
            _anim.SetFloat("Speed", speed);
        }
        else
        {
            _anim.SetFloat("Horizontal", 0);
            _anim.SetFloat("Vertical", 0);
            _anim.SetFloat("Speed", 0);
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
