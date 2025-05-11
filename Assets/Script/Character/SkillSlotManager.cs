using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillSlotKey { Q, E, V }

[System.Serializable]
public class SkillSlot
{
    public SkillSlotKey slotKey;
    public string skillName;
    public float lastUseTime;
    public GameObject slotObject;
    public Image iconImage;
    public Image cooldownOverlay; // = fillImage (Filled Ÿ��)
}

public class SkillSlotManager : MonoBehaviour
{
    public GameObject playerObject;
    public static SkillSlotManager Instance { get; private set; }

    [SerializeField] private List<SkillSlot> slots = new();

    private Dictionary<SkillSlotKey, KeyCode> slotKeyToKeyCode = new()
    {
        { SkillSlotKey.Q, KeyCode.Q },
        { SkillSlotKey.E, KeyCode.E },
        { SkillSlotKey.V, KeyCode.V }
    };

    public void SetPlayer(GameObject player)
    {
        playerObject = player;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        InitializeSlots();
    }

    private void InitializeSlots()
    {
        foreach (var slot in slots)
        {
            if (slot.slotObject != null)
            {
                // �����ܰ� ��Ÿ�� �̹��� ĳ��
                slot.iconImage = slot.slotObject.GetComponent<Image>();
                slot.cooldownOverlay = slot.slotObject.transform.Find("fillimage")?.GetComponent<Image>();

                if (slot.iconImage == null)
                    Debug.LogWarning($"{slot.slotKey}slot: ������ �̹����� �����ϴ�.");
                if (slot.cooldownOverlay == null)
                    Debug.LogWarning($"{slot.slotKey}slot: fillImage ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
    }

    private void Update()
    {
        foreach (var slot in slots)
        {
            SkillData data = SkillManager.Instance.GetPlayerSkill(slot.skillName);
            if (data == null) continue;

            float cooldown = data.Cooldown;
            float elapsed = Time.time - slot.lastUseTime;
            float ratio = Mathf.Clamp01(1f - (elapsed / cooldown));

            // fillAmount ����
            if (slot.cooldownOverlay != null)
                slot.cooldownOverlay.fillAmount = ratio;

            // Ű �Է� ó��
            if (Input.GetKeyDown(slotKeyToKeyCode[slot.slotKey]))
            {
                // ����ź ��ų�� ��Ÿ�� ���� (���� ���� �Ǵ�)
                if (slot.skillName == "Grenade" || ratio <= 0f)
                {
                    UseSkill(slot);
                }
            }
        }
    }

    public void AddSkill(string skillName, Sprite icon)
    {
        foreach (SkillSlotKey key in new[] { SkillSlotKey.Q, SkillSlotKey.E, SkillSlotKey.V })
        {
            SkillSlot slot = slots.Find(s => s.slotKey == key);

            if (slot != null && string.IsNullOrEmpty(slot.skillName))
            {
                slot.skillName = skillName;
                slot.iconImage.sprite = icon;
                slot.lastUseTime = -999f;

                if (slot.cooldownOverlay != null)
                    slot.cooldownOverlay.fillAmount = 0f;
                if (skillName == "Grenade")
                {
                    playerObject.GetComponent<GrenadeSkill>()?.LevelUpSkill();
                }
                Debug.Log($"[SkillSlotManager] {skillName} �� {slot.slotKey} ���Կ� ��ϵ�");
                break;
            }
        }
    }

    private void UseSkill(SkillSlot slot)
    {
        SkillData data = SkillManager.Instance.GetPlayerSkill(slot.skillName);
        if (data == null) return;

        slot.lastUseTime = Time.time;

        switch (slot.skillName)
        {
            case "Pb":
                var pbSkill = playerObject.GetComponent<PenetrationBulletSkill>();
                if (pbSkill != null)
                    pbSkill.TryActivateSkill();
                break;
            case "Eb":
                var ebSkill = playerObject.GetComponent<ExplosiveBulletSkill>();
                if (ebSkill != null)
                    ebSkill.TryActivateSkill();
                break;
            case "Lb":
                var lbSkill = playerObject.GetComponent<ChainLightningSkill>();
                if (lbSkill != null)
                    lbSkill.TryActivateSkill();
                break;
            case "Grenade":
                var grenadeComponent = playerObject.GetComponent<Gunner_grenade>();
                grenadeComponent?.ToggleAimingFromSkill(); // ?? ���� ����
                break;
                // TODO: �ٸ� ��ų ���̽� �߰�
        }
    }
}