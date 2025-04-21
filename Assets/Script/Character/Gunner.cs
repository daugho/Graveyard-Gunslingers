using UnityEngine;

public class Gunner : Player
{
    private bool _isAiming = false;
    [SerializeField] private LayerMask groundLayer;
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }
    protected override void Update()
    {
        base.Update(); // 기본 이동 처리
        HandleAiming();
        HandleRotation();
    }
    private void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _isAiming = true;
            _anim.SetLayerWeight(1, 1);
            //_anim.SetBool("IsAiming", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _isAiming = false;
            _anim.SetLayerWeight(1, 0);
           //_anim.SetBool("IsAiming", false);
        }
    }
    private void RotateToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = transform.position.y; // y 고정해서 수평 회전만

            Vector3 dir = (targetPos - transform.position).normalized;

            if (dir != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, _playerRotateSpeed * Time.deltaTime);
            }
        }
    }
    protected override void HandleMovement()
    {
        _anim.SetFloat("Speed", 0);
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float speed = isSprinting ? _playerMoveSpeed * 3.0f : _playerMoveSpeed;

        if (inputDir.sqrMagnitude > 0.01f)
        {
            transform.Translate(inputDir.normalized * speed * Time.deltaTime, Space.World);
            _anim.SetFloat("Speed", speed);
        }
        else
        {
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
