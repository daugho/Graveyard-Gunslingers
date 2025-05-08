using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxUIController : MonoBehaviour
{
    public static BoxUIController Instance { get; private set; }
    public void OnNormalBoxClick() => OnBoxClick(ItemGrade.Normal);
    public void OnRareBoxClick() => OnBoxClick(ItemGrade.Rare);
    public void OnEpicBoxClick() => OnBoxClick(ItemGrade.Epic);

    [Header("UI 텍스트 연결")]
    [SerializeField] private TMP_Text normalBoxText;
    [SerializeField] private TMP_Text rareBoxText;
    [SerializeField] private TMP_Text epicBoxText;

    [Header("UI 패널")]
    [SerializeField] private GameObject boxUI;

    [Header("버튼 연결")]
    [SerializeField] private Button normalBoxButton;
    [SerializeField] private Button rareBoxButton;
    [SerializeField] private Button epicBoxButton;

    [Header("슬롯머신 오브젝트")]
    [SerializeField] private GameObject normalSlotMachine;
    [SerializeField] private GameObject rareSlotMachine;
    [SerializeField] private GameObject epicSlotMachine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        boxUI.SetActive(false);

        normalBoxButton.onClick.AddListener(() => OnBoxClick(ItemGrade.Normal));
        rareBoxButton.onClick.AddListener(() => OnBoxClick(ItemGrade.Rare));
        epicBoxButton.onClick.AddListener(() => OnBoxClick(ItemGrade.Epic));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (RoundManager.Instance != null && RoundManager.Instance.GetAliveMonsterCount() <= 0)
            {
                bool isActive = boxUI.activeSelf;
                boxUI.SetActive(!isActive);

                if (!isActive) // UI가 새로 열릴 때만 갱신
                    UpdateBoxUI();
            }
        }
    }

    public void UpdateBoxUI()
    {
        normalBoxText.text = Inventory.Instance.GetBoxCount(ItemGrade.Normal).ToString();
        rareBoxText.text = Inventory.Instance.GetBoxCount(ItemGrade.Rare).ToString();
        epicBoxText.text = Inventory.Instance.GetBoxCount(ItemGrade.Epic).ToString();
    }


    private void OnBoxClick(ItemGrade grade)
    {
       // if (Inventory.Instance.GetBoxCount(grade) <= 0)
       // {
       //     Debug.Log($"[BoxUIController] {grade} 박스 없음");
       //     return;
       // }
       //
       // Inventory.Instance.AddBox(grade, -1); // 박스 하나 소모
       // UpdateBoxUI();

        // 슬롯머신 열기 전 다른 슬롯머신 UI 닫기
        normalSlotMachine.SetActive(false);
        rareSlotMachine.SetActive(false);
        epicSlotMachine.SetActive(false);

        // 해당 등급 슬롯머신 열기
        switch (grade)
        {
            case ItemGrade.Normal:
                normalSlotMachine.SetActive(true);
                break;
            case ItemGrade.Rare:
                rareSlotMachine.SetActive(true);
                break;
            case ItemGrade.Epic:
                epicSlotMachine.SetActive(true);
                break;
        }

    }

    public void CloseUI()
    {
        boxUI.SetActive(false);
    }
}