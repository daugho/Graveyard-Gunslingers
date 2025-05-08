using UnityEngine;

public class RoundClear : MonoBehaviour
{
    public void KillAllMonsters()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        foreach (var monster in monsters)
        {
            monster.TakeDamage(999999f); // ū �������� �༭ ��� ���
        }

        Debug.Log($"[DebugTools] {monsters.Length}���� ���� ���ŵ�");
    }
}
