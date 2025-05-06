using UnityEngine;

public class Gunner_grenade : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LineRenderer trajectoryRenderer;
    [SerializeField] private GameObject arrowIndicator;
    [SerializeField] private int segmentCount = 20;

    private bool isAiming = false;
    private GameObject warningEffect;
    private Vector3 currentVelocity;

    private void Start()
    {
        if (trajectoryRenderer == null)
        {
            trajectoryRenderer = GameObject.Find("GrenadeTrajectory")?.GetComponent<LineRenderer>();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isAiming = !isAiming;

            if (isAiming)
            {
                warningEffect = EffectPool.Instance.GetEffect("Warning");
            }
            else
            {
                if (warningEffect != null)
                {
                    EffectPool.Instance.ReturnEffect("Warning", warningEffect);
                    warningEffect = null;
                }

                trajectoryRenderer.positionCount = 0;
                arrowIndicator.SetActive(false);
            }
        }

        if (isAiming)
        {
            UpdateAiming();

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 targetPos = warningEffect.transform.position;
                ThrowGrenade(targetPos);
                EffectPool.Instance.ReturnEffect("Warning", warningEffect);
                warningEffect = null;
                isAiming = false;
            }
        }
    }
    private void UpdateAiming()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 targetPos = hit.point + Vector3.up * 0.1f;
            warningEffect.transform.position = hit.point + Vector3.up * 0.1f;
            warningEffect.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            currentVelocity = CalculateParabolaVelocity(firePoint.position, targetPos, 1.0f);
            DrawTrajectory(firePoint.position, currentVelocity);
        }
        else
        {
            Debug.LogWarning("���� ��ġ ���� ����: ������ �������� ����");
        }
    }
    private void DrawTrajectory(Vector3 startPos, Vector3 velocity)
    {
        Vector3[] points = new Vector3[segmentCount];

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i * 0.1f;
            Vector3 point = startPos + velocity * t + 0.5f * Physics.gravity * t * t;
            points[i] = point;
        }

        trajectoryRenderer.positionCount = segmentCount;
        trajectoryRenderer.SetPositions(points);

        Vector3 endPos = points[segmentCount - 1];
        Vector3 prevPos = points[segmentCount - 2];
        Vector3 dir = (endPos - prevPos).normalized;

        arrowIndicator.transform.position = endPos + Vector3.up * 0.05f;
        arrowIndicator.SetActive(true);
    }

    private void ThrowGrenade(Vector3 targetPosition)// ������ ������ ����Ͽ� ����ź�� ������ �Լ�
    {
        GameObject grenade = GrenadePool.Instance.GetGrenade(); // �� Ǯ���� ������
        grenade.transform.position = firePoint.position;
        grenade.transform.rotation = Quaternion.identity;

        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = currentVelocity;

        trajectoryRenderer.positionCount = 0;
        arrowIndicator.SetActive(false);
    }

    private Vector3 CalculateParabolaVelocity(Vector3 start, Vector3 end, float timeToTarget)
    {
        Vector3 distance = end - start;// �������� ���� ������ �Ÿ� v = d / t
        Vector3 distanceXZ = new Vector3(distance.x, 0f, distance.z);//��ӵ� � ���� y = v?��t + (1/2)��a��t��
        float yOffset = distance.y;
        float verticalSpeed = yOffset / timeToTarget + 0.5f * Mathf.Abs(Physics.gravity.y) * timeToTarget;
        Vector3 horizontalSpeed = distanceXZ / timeToTarget;

        return horizontalSpeed + Vector3.up * verticalSpeed;//���� + ���� ���͸� ���ļ� ��ȯ.
    }
}
