using UnityEngine;

public class QuarterViewCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10, -10);
    public float followSpeed = 5f;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.2f;
    private float dampingSpeed = 1.0f;

    private void Update()
    {
        if (target == null) return;

        Vector3 basePosition = target.position + offset;
        Vector3 shakeOffset = Vector3.zero;

        if (shakeDuration > 0)
        {
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }

        transform.position = basePosition + shakeOffset;
        transform.LookAt(target);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void TriggerShake(float duration = 0.3f)
    {
        shakeDuration = duration;
    }

}
