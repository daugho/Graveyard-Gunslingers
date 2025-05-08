using UnityEngine;

public class RoundClear : MonoBehaviour
{
    public void KillAllMonsters()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        foreach (var monster in monsters)
        {
            monster.TakeDamage(999999f); // 큰 데미지를 줘서 즉시 사망
        }

        Debug.Log($"[DebugTools] {monsters.Length}마리 몬스터 제거됨");
    }
}
