using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Gunner_HpExp : MonoBehaviour, IDamageable
{
    private Slider _healthSlider;
    private Slider _expSlider;
    private TextMeshProUGUI _hpText;
    private TextMeshProUGUI _expText;
    private float _maxHealth;
    private float _currentHealth;
    private float _curExp;
    private Renderer[] _renderers;
    private RawImage _deathScreen;
    private bool _isFadingOut = true;
    private Player_Gunner _playerGunner;

    void Start()
    {
        _playerGunner = GetComponent<Player_Gunner>();

        if (_playerGunner == null || _playerGunner.Stats == null)
        {
            Debug.LogError("[Gunner_HpExp] Player_Gunner 또는 Stats가 초기화되지 않았습니다.");
            return;
        }

        _maxHealth = _playerGunner.Stats._health;
        _currentHealth = _maxHealth;
        _curExp = 0;

        _healthSlider = GameObject.Find("HpBar").GetComponent<Slider>();
        Transform hpTextTransform = _healthSlider.transform.GetChild(2); // ← 0부터 시작, 3번째 자식은 index 2
        _hpText = hpTextTransform.GetComponent<TextMeshProUGUI>();

        _expSlider = GameObject.Find("ExpBar").GetComponent<Slider>();
        Transform expTextTransform = _expSlider.transform.GetChild(2); // ← 0부터 시작, 3번째 자식은 index 2
        _expText = expTextTransform.GetComponent<TextMeshProUGUI>();


        _expSlider.value = 0;
        _expText.text = $"{(_curExp).ToString("F2")}%";

        if (_healthSlider != null)
        {
            _healthSlider.maxValue = _maxHealth;
            _healthSlider.value = _currentHealth;
            _hpText.text = $"{(_currentHealth).ToString("F0")}";
        }
        _renderers = GetComponentsInChildren<Renderer>();

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            GetExp(20f);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10000f);
        }
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_currentHealth < 0)
            Die();
        if (_healthSlider != null)
            _healthSlider.value = _currentHealth;

        if (_hpText != null)
            _hpText.text = $"{_currentHealth} / {_maxHealth}";

    }
    public void GetExp(float exp)
    {
        _curExp += exp;
        if (_curExp >= _playerGunner.Stats._exp)
        {
            int nextLevel = _playerGunner.Stats._level + 1;
            _playerGunner.LevelUp(nextLevel);
            _curExp %= _playerGunner.Stats._exp;
            _curExp = 0;
        }

        _expText.text = $"{(_curExp / _playerGunner.Stats._exp * 100f):F2}%";
        _expSlider.value = _curExp / _playerGunner.Stats._exp;
    }
    private void Die()
    {
        Debug.Log("[플레이어] 사망");
        // 죽었을 때 처리 (애니메이션 재생, GameOver 띄우기 등)
    }
}
