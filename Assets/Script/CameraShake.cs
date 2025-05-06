using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float shakeDuration = 0.3f;
    private float shakeMagnitude = 0.2f;
    private float dampingSpeed = 1.0f;

    private float currentShakeTime = 0f;
    private Vector3 initialPosition;

    private void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (currentShakeTime > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            currentShakeTime -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            currentShakeTime = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void TriggerShake(float duration = -1f)
    {
        currentShakeTime = duration > 0 ? duration : shakeDuration;
    }
}