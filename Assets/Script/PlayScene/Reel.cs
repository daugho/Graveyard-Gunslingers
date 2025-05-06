using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
public class Reel : MonoBehaviour
{
    public List<Sprite> iconSprites;           // 슬롯 아이콘들
    public GameObject iconPrefab;              // 프리팹 (Image 포함)
    public RectTransform iconContainer;        // 아이콘 부모 (RectTransform)
    public float iconSpacing = 150f;           // 아이콘 간격
    public float spinDuration = 2f;

    private List<GameObject> icons = new();

    public void Initialize()
    {
        foreach (Transform child in iconContainer)
            Destroy(child.gameObject);

        icons.Clear();

        for (int i = 0; i < iconSprites.Count; i++)
        {
            var icon = Instantiate(iconPrefab, iconContainer);
            icon.GetComponent<Image>().sprite = iconSprites[i];
            icons.Add(icon);
        }

        iconContainer.anchoredPosition = Vector2.zero;
    }

    public void SpinToResult(int resultIndex)
    {
        float targetY = resultIndex * iconSpacing;
        float fullSpin = iconSprites.Count * iconSpacing * 5f;
        float finalY = targetY + fullSpin;

        iconContainer.DOAnchorPosY(-finalY, spinDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                iconContainer.anchoredPosition = new Vector2(0, -targetY);
            });
    }
}