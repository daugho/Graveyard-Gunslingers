using UnityEngine;
using UnityEngine.UI;

public class NormalSlotMachine : MonoBehaviour
{
    [SerializeField] private Image[] slotImages;
    [SerializeField] private Sprite[] normalSprites;

    [Header("결과 인덱스 → 스킬 이름 매핑")]
    [SerializeField] private string[] skillNames; // 예: ["관통탄", "수류탄", "이중실린더"]

    [Header("애니메이션 설정")]
    [SerializeField] private int stepCount = 20;
    [SerializeField] private float startDelay = 0.05f;
    [SerializeField] private float endDelay = 0.8f;

    private SlotMachineLogic _controller = new SlotMachineLogic();

    private void Awake()
    {
        _controller.Setup(slotImages, normalSprites, 1, stepCount, startDelay, endDelay);

        _controller.OnShuffleComplete = () =>
        {

            int resultIndex = _controller.ResultIndexes[0];
            string skillName = skillNames[resultIndex];

            SkillManager.Instance.AddOrLevelUpSkill(skillName);

            Debug.Log($"[NormalSlotMachine] 슬롯 결과 → Index: {resultIndex}, SkillName: {skillName}");

            Invoke(nameof(DisableSelf), 1.5f);
        };
    }

    public void OnButtonClick()
    {
        _controller.Shuffle();
        StartCoroutine(_controller.ShuffleAnimation());
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}