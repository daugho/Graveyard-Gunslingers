using UnityEngine;

public class Gunner_grenade : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LineRenderer trajectoryRenderer;
    [SerializeField] private GameObject arrowIndicator;
    [SerializeField] private int segmentCount = 20;

    private bool _isAiming = false;
    private GameObject _warningEffect;
    private Vector3 _currentVelocity;
    private Vector3 _targetPosition;

    private void Start()
    {
        if (trajectoryRenderer == null)
        {
            trajectoryRenderer = GameObject.Find("GrenadeTrajectory")?.GetComponent<LineRenderer>();
        }
    }

    void Update()
    {
        if (_isAiming)
        {
            UpdateAiming();

            if (Input.GetMouseButtonDown(0))
            {
                // 수류탄 발사 시도 (보유 수 확인 포함)
                GetComponent<GrenadeSkill>()?.TryUseGrenade();
            }
        }
    }
    public void ToggleAimingFromSkill()
    {
        int currentCount = GetComponent<GrenadeSkill>()?.GetCurrentCount() ?? 0;
        if (currentCount <= 0)
        {
            Debug.Log("❌ 수류탄이 없어서 조준 모드에 진입하지 않습니다.");
            return;
        }

        ToggleAiming(); // 기존 조준 진입
    }
    private void ToggleAiming()
    {
        _isAiming = !_isAiming;

        if (_isAiming)
        {
            _warningEffect = EffectPool.Instance.GetEffect("Warning");
        }
        else
        {
            if (_warningEffect != null)
            {
                EffectPool.Instance.ReturnEffect("Warning", _warningEffect);
                _warningEffect = null;
            }

            trajectoryRenderer.positionCount = 0;
            arrowIndicator.SetActive(false);
        }
    }

    private void UpdateAiming()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 targetPos = hit.point + Vector3.up * 0.1f;
            _warningEffect.transform.position = targetPos;
            _warningEffect.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            _currentVelocity = CalculateParabolaVelocity(firePoint.position, targetPos, 1.0f);
            DrawTrajectory(firePoint.position, _currentVelocity);
        }
    }

    private void DrawTrajectory(Vector3 startPos, Vector3 velocity)
    {
        Vector3[] points = new Vector3[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            float t = i * 0.1f;
            points[i] = startPos + velocity * t + 0.5f * Physics.gravity * t * t;
        }

        trajectoryRenderer.positionCount = segmentCount;
        trajectoryRenderer.SetPositions(points);

        Vector3 endPos = points[segmentCount - 1];
        Vector3 prevPos = points[segmentCount - 2];
        Vector3 dir = (endPos - prevPos).normalized;

        arrowIndicator.transform.position = endPos + Vector3.up * 0.05f;
        arrowIndicator.SetActive(true);
    }

    // 외부에서 호출되는 수류탄 던지기 메서드
    public void ThrowWithCurrentVelocity()
    {
        GameObject grenade = GrenadePool.Instance.GetGrenade();
        grenade.transform.position = firePoint.position;
        grenade.transform.rotation = Quaternion.identity;

        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = _currentVelocity;

        float damage = GetComponent<GrenadeSkill>()?.GetGrenadeDamage() ?? 0f;
        grenade.GetComponent<GrenadeExplode>()?.SetDamage(damage);

        trajectoryRenderer.positionCount = 0;
        arrowIndicator.SetActive(false);

        if (_warningEffect != null)
        {
            EffectPool.Instance.ReturnEffect("Warning", _warningEffect);
            _warningEffect = null;
        }

        _isAiming = false;
    }

    private Vector3 CalculateParabolaVelocity(Vector3 start, Vector3 end, float timeToTarget)
    {
        Vector3 distance = end - start;
        Vector3 distanceXZ = new Vector3(distance.x, 0f, distance.z);
        float yOffset = distance.y;

        float verticalSpeed = yOffset / timeToTarget + 0.5f * Mathf.Abs(Physics.gravity.y) * timeToTarget;
        Vector3 horizontalSpeed = distanceXZ / timeToTarget;

        return horizontalSpeed + Vector3.up * verticalSpeed;
    }
}
