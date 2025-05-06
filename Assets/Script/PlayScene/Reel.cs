using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
public class Reel : MonoBehaviour
{
    public List<Sprite> iconSprites;           // ���� �����ܵ�
    public GameObject iconPrefab;              // ������ (Image ����)
    public RectTransform iconContainer;        // ������ �θ� (RectTransform)
    public float iconSpacing = 150f;           // ������ ����
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