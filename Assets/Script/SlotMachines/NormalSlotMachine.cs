using UnityEngine;
using UnityEngine.UI;

public class NormalSlotMachine : MonoBehaviour
{
    [SerializeField] private Image[] slotImages;
    [SerializeField] private Sprite[] normalSprites;

    [Header("�ִϸ��̼� ����")]
    [SerializeField] private int stepCount = 20;
    [SerializeField] private float startDelay = 0.05f;
    [SerializeField] private float endDelay = 0.8f;

    [Header("����Ʈ ����")]
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

            Invoke(nameof(DisableSelf), 1.5f); // 1.5�� �� ȭ�� ����
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