using UnityEngine;
using UnityEngine.UI;

public class NormalSlotMachine : MonoBehaviour
{
    [SerializeField] private Image[] slotImages;
    [SerializeField] private Sprite[] normalSprites;

    [Header("애니메이션 설정")]
    [SerializeField] private int stepCount = 20;
    [SerializeField] private float startDelay = 0.05f;
    [SerializeField] private float endDelay = 0.8f;

    [Header("이펙트 연결")]
    [SerializeField] private GameObject rewardEffect;

    private SlotMachineLogic _controller = new SlotMachineLogic();

    private void Awake()
    {
        _controller.Setup(slotImages, normalSprites, 1, stepCount, startDelay, endDelay);

        _controller.OnShuffleComplete = () =>
        {
            if (rewardEffect != null)
            {
                rewardEffect.SetActive(true);
            }

            Invoke(nameof(DisableSelf), 1.5f); // 1.5초 후 화면 끄기
            rewardEffect.SetActive(false);

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