using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(DecalProjector))]
public class WarningEffect : MonoBehaviour
{
    public float expandSpeed = 3f;
    public float maxSize = 4f;
    public float duration = 2f;

    private DecalProjector projector;
    private float timer = 0f;

    [Header("Outline Reference")]
    public DecalProjector outlineProjector;
    private void Awake()
    {
        projector = GetComponent<DecalProjector>();
        projector.size = new Vector3(0.1f, 0.1f, 1f);
        if (outlineProjector != null)
        {
            outlineProjector.size = new Vector3(maxSize, maxSize, 1f);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // 1. 크기 점점 키우기
        float sizeT = Mathf.Clamp01(timer / duration);
        float size = Mathf.Lerp(0.1f, maxSize, sizeT);
        projector.size = new Vector3(size, size, 1f);
    }
    private void OnDisable()
    {
        if (projector != null)
            projector.size = new Vector3(0.1f, 0.1f, 1f);

        timer = 0f;
    }
}