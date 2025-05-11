using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SlotMachineLogic
{
    public Action OnShuffleComplete;
    protected Image[] slotImages;
    protected Sprite[] possibleSprites;
    protected int resultCount;

    protected int stepCount;
    protected float startDelay;
    protected float endDelay;

    protected int[] resultIndexes;
    public int[] ResultIndexes => resultIndexes;
    protected bool isShuffling;
    public void Setup(Image[] slotImages, Sprite[] possibleSprites, int resultCount, int stepCount, float startDelay, float endDelay)
    {
        this.slotImages = slotImages;
        this.possibleSprites = possibleSprites;
        this.resultCount = resultCount;
        this.stepCount = stepCount;
        this.startDelay = startDelay;
        this.endDelay = endDelay;
    }

    public void Shuffle()
    {
        if (isShuffling || possibleSprites.Length == 0)
            return;

        resultIndexes = new int[resultCount];//보상 개수 슬롯
        for (int i = 0; i < resultCount; i++)
            resultIndexes[i] = UnityEngine.Random.Range(0, possibleSprites.Length);
        isShuffling = true;
    }

    public IEnumerator ShuffleAnimation()
    {
        for (int i = 0; i < stepCount; i++)
        {
            float t = (float)i / (stepCount - 1);
            float delay = Mathf.Lerp(startDelay, endDelay, 1 - Mathf.Pow(1 - t, 3));

            for (int j = 0; j < resultCount; j++)
            {
                slotImages[j].sprite = possibleSprites[UnityEngine.Random.Range(0, possibleSprites.Length)];
            }

            yield return new WaitForSeconds(delay);
        }

        for (int i = 0; i < resultCount; i++)
        {
            slotImages[i].sprite = possibleSprites[resultIndexes[i]];
        }

        isShuffling = false;
        OnShuffleComplete?.Invoke();
    }
}

//public class SlotMachineController : MonoBehaviour
//{
//    [Header("슬롯 구성 요소")]
//    [SerializeField] private Image _slotImage;             // 한 개의 슬롯 이미지
//    [SerializeField] private Image _resultImage;           // 결과 표시 이미지
//    [SerializeField] private Sprite[] _itemSprites;        // 슬롯에 사용할 스프라이트
//
//    [Header("슬롯 동작 설정")]
//    [SerializeField] private int _stepCount = 20;          // 이미지 변경 횟수
//    [SerializeField] private float _startDelay = 0.05f;    // 초기 변경 간격
//    [SerializeField] private float _endDelay = 0.8f;       // 마지막 변경 간격
//
//    [Header("박스 등급별 슬롯 스프라이트")]
//    [SerializeField] private Sprite[] normalSprites;
//    [SerializeField] private Sprite[] rareSprites;//
//    [SerializeField] private Sprite[] epicSprites;
//
//    public bool IsSpinning { get; private set; }
//    public int ResultIndex { get; private set; }
//
//    public void OnButtonClick()
//    {
//        if (!IsSpinning)
//        {
//            ResultIndex = Random.Range(0, _itemSprites.Length);
//            StartCoroutine(ShuffleImages());
//        }
//    }
//    private IEnumerator ShuffleImages()
//    {
//        IsSpinning = true;
//
//        for (int i = 0; i < _stepCount; i++)
//        {
//            float t = (float)i / (_stepCount - 1);
//            float delay = Mathf.Lerp(_startDelay, _endDelay, 1 - Mathf.Pow(1 - t, 3)); // EaseOutCubic
//
//            _slotImage.sprite = _itemSprites[Random.Range(0, _itemSprites.Length)];
//            yield return new WaitForSeconds(delay);
//        }
//
//        _slotImage.sprite = _itemSprites[ResultIndex];
//        _resultImage.sprite = _itemSprites[ResultIndex];
//        IsSpinning = false;
//    }
//}

//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//
//public class SlotMachineController : MonoBehaviour
//{
//    [Header("슬롯 구성 요소")]
//    [SerializeField] private RectTransform[] _slotImages;
//    [SerializeField] private Image _resultImage;
//    [SerializeField] private Sprite[] _itemSprites;
//
//    [Header("슬롯 동작 설정")]
//    [SerializeField] private float _spinDuration = 2f;
//    [SerializeField] private float _scrollSpeed = 300f;
//
//    public bool IsSpinning { get; private set; }
//    public int ResultIndex { get; private set; }
//
//    private const float _slotHeight = 200f;
//
//    public void OnButtonClick()
//    {
//        if (!IsSpinning)
//        {
//            ResultIndex = Random.Range(0, _itemSprites.Length);
//            StartCoroutine(SpinRoutine());
//        }
//    }
//
//    private IEnumerator SpinRoutine()
//    {
//        IsSpinning = true;
//        float timer = 0f;
//        float fastSpinTime = _spinDuration * 0.5f;
//
//        // 1. 빠른 회전 단계
//        while (timer < fastSpinTime)
//        {
//            float speed = _scrollSpeed;
//            for (int i = 0; i < _slotImages.Length; i++)
//            {
//                _slotImages[i].anchoredPosition -= new Vector2(0f, speed * Time.deltaTime);
//
//                if (_slotImages[i].anchoredPosition.y <= -_slotHeight * 1.5f)
//                {
//                    float maxY = GetMaxY();
//                    _slotImages[i].anchoredPosition = new Vector2(
//                        _slotImages[i].anchoredPosition.x,
//                        maxY + _slotHeight);
//                    CycleSprite(_slotImages[i]);
//                }
//            }
//            timer += Time.deltaTime;
//            yield return null;
//        }
//
//        // 2. 감속 정렬 단계
//        yield return StartCoroutine(SlowStepToResult(ResultIndex));
//
//        // 3. 결과 스프라이트 지정
//        _resultImage.sprite = _itemSprites[ResultIndex];
//        IsSpinning = false;
//    }
//
//    private IEnumerator SlowStepToResult(int resultIndex)
//    {
//        int currentIndex = System.Array.IndexOf(_itemSprites, _slotImages[0].GetComponent<Image>().sprite);
//        int stepCount = (_itemSprites.Length + resultIndex - currentIndex) % _itemSprites.Length;
//        float delay = 0.1f;
//
//        for (int i = 0; i <= stepCount; i++)
//        {
//            for (int j = 0; j < _slotImages.Length; j++)
//            {
//                Vector2 pos = _slotImages[j].anchoredPosition;
//                pos.y -= _slotHeight;
//                _slotImages[j].anchoredPosition = pos;
//
//                if (pos.y <= -_slotHeight * 1.5f)
//                {
//                    float maxY = GetMaxY();
//                    Vector2 reset = pos;
//                    reset.y = maxY + _slotHeight;
//                    _slotImages[j].anchoredPosition = reset;
//
//                    currentIndex = (currentIndex + 1) % _itemSprites.Length;
//                    _slotImages[j].GetComponent<Image>().sprite = _itemSprites[currentIndex];
//                }
//            }
//            yield return new WaitForSeconds(delay);
//            delay += 0.02f;
//        }
//    }
//
//    private float GetMaxY()
//    {
//        float max = float.MinValue;
//        foreach (RectTransform rt in _slotImages)
//        {
//            if (rt.anchoredPosition.y > max)
//                max = rt.anchoredPosition.y;
//        }
//        return max;
//    }
//
//    private void CycleSprite(RectTransform slot)
//    {
//        Image img = slot.GetComponent<Image>();
//        int currentIndex = System.Array.IndexOf(_itemSprites, img.sprite);
//        int nextIndex = (currentIndex + 1) % _itemSprites.Length;
//        img.sprite = _itemSprites[nextIndex];
//    }
//}
