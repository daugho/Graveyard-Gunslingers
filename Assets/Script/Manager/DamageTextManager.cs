using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance { get; private set; }

    [SerializeField] private GameObject damageTextPrefab; // UI/DMGText «¡∏Æ∆’
    [SerializeField] private Transform canvasTransform;   // DamageCanvas¿« transform

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (damageTextPrefab == null)
            damageTextPrefab = Resources.Load<GameObject>("DMGPrefab/DMGText");

        if (canvasTransform == null)
        {
            GameObject canvasObj = GameObject.Find("DamageCanvas");
            if (canvasObj != null)
                canvasTransform = canvasObj.transform;
            else { }
        }
    }

    public void Show(float damage, Vector3 worldPos, DamageType type)
    {
        if (damageTextPrefab == null || canvasTransform == null)
        {

            return;
        }

        GameObject instance = Instantiate(damageTextPrefab, canvasTransform);
        instance.transform.position = worldPos;

        DamageText text = instance.GetComponent<DamageText>();
        if (text != null)
        {
            text.Initialize(damage, type);
        }
    }
}
