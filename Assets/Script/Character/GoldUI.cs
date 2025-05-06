using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public static GoldUI Instance;

    [SerializeField] private TMP_Text goldText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateGold(int amount)
    {
        goldText.text = $"Gold: {amount}";
    }
}