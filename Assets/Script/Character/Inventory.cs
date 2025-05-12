using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private const string GOLD_KEY = "PlayerGold";
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
        // 골드 로드
        goldAmount = PlayerPrefs.GetInt(GOLD_KEY, 0);

        // 박스 초기화
        foreach (ItemGrade grade in System.Enum.GetValues(typeof(ItemGrade)))
            boxCount[grade] = 0;
    }

    public void AddGold(int amount)
    {
        goldAmount += amount;
        PlayerPrefs.SetInt(GOLD_KEY, goldAmount);
        PlayerPrefs.Save();


        GoldUI.Instance?.UpdateGold(goldAmount);
    }

    public int GetGold() => goldAmount;

    public void AddBox(ItemGrade grade, int count = 1)
    {
        if (!boxCount.ContainsKey(grade))
            boxCount[grade] = 0;

        boxCount[grade] += count;

        BoxUIController.Instance?.UpdateBoxUI();
    }

    public int GetBoxCount(ItemGrade grade) => boxCount.TryGetValue(grade, out int count) ? count : 0;
}
