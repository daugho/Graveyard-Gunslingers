using System.Collections;
using UnityEngine;

public class RevolverUI : MonoBehaviour
{
    //PHYSICE over-> 충돌 범위
    [SerializeField] private Transform cylinder;
    private GameObject[] bulletSlots = new GameObject[6];
    private int currentRotationIndex = 0; // 현재 몇 발 쐈는지 기록
    private bool isRotating = false;

    private void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            bulletSlots[i] = cylinder.GetChild(i).gameObject;
        }
    }
    public void Fire()
    {
        //if (isRotating) return;

        if (bulletSlots[currentRotationIndex].activeSelf)
        {
            bulletSlots[currentRotationIndex].SetActive(false);
        }

        currentRotationIndex++;
        if (currentRotationIndex == 6)
            currentRotationIndex = 0;

        StartCoroutine(RotateSmoothly(60f, 0.2f)); // 0.3초 동안 60도 회전
    }
    private IEnumerator RotateSmoothly(float deltaAngle, float duration)
    {
        isRotating = true;

        Quaternion startRot = cylinder.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, 0f, deltaAngle);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cylinder.localRotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }

        cylinder.localRotation = endRot;
        isRotating = false;
    }

    public void ReloadAll()
    {
        foreach (var slot in bulletSlots)
            slot.SetActive(true);

        currentRotationIndex = 0;
        cylinder.localRotation = Quaternion.identity;
    }
}

