using UnityEngine;
using UnityEngine.UI;

public class RereSlotMachine : MonoBehaviour
{
    [SerializeField] private Image[] slotImages;
    [SerializeField] private Sprite[] normalSprites;

    [Header("애니메이션 설정")]
    [SerializeField] private int stepCount = 20;
    [SerializeField] private float startDelay = 0.05f;
    [SerializeField] private float endDelay = 0.8f;

    private SlotMachineLogic _controller = new SlotMachineLogic();

    private void Awake()
    {
        _controller.Setup(slotImages, normalSprites, 3, stepCount, startDelay, endDelay);
    }

    public void OnButtonClick()
    {
        _controller.Shuffle();
        StartCoroutine(_controller.ShuffleAnimation());
    }
}
