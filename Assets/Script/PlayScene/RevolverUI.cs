using UnityEngine;

public class RevolverUI : MonoBehaviour
{
    //PHYSICE over-> �浹 ����
    [SerializeField] private Transform cylinder; // �Ǹ��� UI
    [SerializeField] private GameObject[] bulletSlots = new GameObject[6]; // �Ѿ� ����
    private int currentSlot = 0;

    public void Fire()
    {
        // �Ѿ� ��Ȱ��ȭ
        if (bulletSlots[currentSlot].activeSelf)
            bulletSlots[currentSlot].SetActive(false);

        // ȸ�� (60���� �ð� ����)
        //cylinder.Rotate(0f, 0f, -60f);
        cylinder.rotation = Quaternion.Euler(0,0,-60.0f);

        // ���� �������� �̵� (0~5 �ݺ�)
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

