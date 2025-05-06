using TMPro;
using UnityEngine;

public class BoxUIController : MonoBehaviour
{
    public static BoxUIController Instance;

    [Header("UI 텍스트 연결")]
    [SerializeField] private TMP_Text normalBoxText;
    [SerializeField] private TMP_Text rareBoxText;
    [SerializeField] private TMP_Text epicBoxText;

    [Header("UI 패널")]
    [SerializeField] private GameObject boxUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        boxUI.SetActive(false); // 시작 시 꺼짐
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (RoundManager.Instance != null && RoundManager.Instance.GetAliveMonsterCount() <= 0)
            {
                bool isActive = boxUI.activeSelf;
                boxUI.SetActive(!isActive);

                if (!isActive) // 열릴 때만 갱신
                {
                    UpdateBoxUI();
                }
            }
        }
    }

    public void UpdateBoxUI()
    {
        normalBoxText.text = Inventory.Instance.GetBoxCount(ItemGrade.Normal).ToString();
        rareBoxText.text = Inventory.Instance.GetBoxCount(ItemGrade.Rare).ToString();
        epicBoxText.text = Inventory.Instance.GetBoxCount(ItemGrade.Epic).ToString();
    }

    public void CloseUI()
    {
        boxUI.SetActive(false);
    }
}