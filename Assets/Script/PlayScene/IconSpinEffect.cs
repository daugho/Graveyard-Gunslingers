using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSpinEffect : MonoBehaviour
{
    public List<Sprite> spinSprites;
    public float spinInterval = 0.05f;

    private Image _image;
    private Coroutine _spinRoutine;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void StartSpin()
    {
        _spinRoutine = StartCoroutine(Spin());
    }

    public void StopSpin(Sprite finalSprite)
    {
        if (_spinRoutine != null)
            StopCoroutine(_spinRoutine);

        _image.sprite = finalSprite;
    }

    private IEnumerator Spin()
    {
        while (true)
        {
            _image.sprite = spinSprites[Random.Range(0, spinSprites.Count)];
            yield return new WaitForSeconds(spinInterval);
        }
    }
}
