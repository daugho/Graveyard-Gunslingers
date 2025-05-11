using TMPro;
using UnityEngine;

public enum DamageType
{
    Normal,
    Penetration,
    Explosive,
    ChainLightning,
    Critical
}

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    private float floatSpeed = 1f;
    private float lifeTime = 1f;

    public void Initialize(float damage, DamageType type)
    {
        damageText.text = damage.ToString("F0");
        damageText.color = GetColor(type);
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime, Space.World);
    }

    private Color GetColor(DamageType type)
    {
        return type switch
        {
            DamageType.Normal => Color.white,
            DamageType.Penetration => new Color(0.4f, 0.8f, 1f), // ¹àÀº ÆÄ¶û
            DamageType.Explosive => new Color(1f, 0.5f, 0.2f), // ÁÖÈ²
            DamageType.ChainLightning => Color.cyan,
            DamageType.Critical => Color.red,
            _ => Color.white
        };
    }
}
