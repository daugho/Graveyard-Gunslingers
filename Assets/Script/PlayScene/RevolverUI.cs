using UnityEngine;

public class RevolverUI : MonoBehaviour
{
    //PHYSICE over-> 충돌 범위
    [SerializeField] private Transform cylinder; // 실린더 UI
    [SerializeField] private GameObject[] bulletSlots = new GameObject[6]; // 총알 슬롯
    private int currentSlot = 0;

    public void Fire()
    {
        // 총알 비활성화
        if (bulletSlots[currentSlot].activeSelf)
            bulletSlots[currentSlot].SetActive(false);

        // 회전 (60도씩 시계 방향)
        //cylinder.Rotate(0f, 0f, -60f);
        cylinder.rotation = Quaternion.Euler(0,0,-60.0f);

        // 다음 슬롯으로 이동 (0~5 반복)
        currentSlot = (currentSlot + 1) % bulletSlots.Length;
    }

    public void ReloadAll()
    {
        foreach (var slot in bulletSlots)
            slot.SetActive(true);
        currentSlot = 0;
        cylinder.localRotation = Quaternion.identity;
    }
}

