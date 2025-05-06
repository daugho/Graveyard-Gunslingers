using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private int goldAmount = 0;
    private Dictionary<ItemGrade, int> boxCount = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        GoldUI.Instance?.UpdateGold(goldAmount);
    }

    private void Initialize()
    {
        foreach (ItemGrade grade in System.Enum.GetValues(typeof(ItemGrade)))
            boxCount[grade] = 0;
    }

    public void AddGold(int amount)
    {
        goldAmount += amount;
        Debug.Log($"[Inventory] °ñµå +{amount} ¡æ ÃÑÇÕ: {goldAmount}");

        GoldUI.Instance?.UpdateGold(goldAmount);
    }

    public void AddBox(ItemGrade grade, int count = 1)
    {
        if (!boxCount.ContainsKey(grade))
            boxCount[grade] = 0;

        boxCount[grade] += count;
        Debug.Log($"[Inventory] ¹Ú½º È¹µæ: {grade} x{count} ¡æ ÃÑ: {boxCount[grade]}");

        BoxUIController.Instance?.UpdateBoxUI();
    }

    public int GetGold() => goldAmount;

    public int GetBoxCount(ItemGrade grade) => boxCount.TryGetValue(grade, out int count) ? count : 0;
}