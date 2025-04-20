using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TweenScale _currentTween;

    public void OnPointerEnter(PointerEventData eventData)
    {
        var tw = TweenScale.Begin(_currentTween.gameObject, new Vector3(1, 1, 1), new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _currentTween?.Play(false);
    }
}
