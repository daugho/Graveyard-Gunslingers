using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Gunner_HpExp : MonoBehaviour
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

        //GameObject obj = GameObject.Find("DeathScreen");
        //if (obj != null)
        //{
        //    _deathScreen = obj.GetComponent<RawImage>();
        //
        //    if (_deathScreen != null)
        //    {
        //
        //        Color c = _deathScreen.color;
        //        c.a = 0.0f;
        //        _deathScreen.color = c;
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            GetExp(20f); // G 키 입력 시 경험치 20 추가
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

        if (_healthSlider != null)
            _healthSlider.value = _currentHealth;

        if (_hpText != null)
            _hpText.text = $"{_currentHealth} / {_maxHealth}";

        //StartCoroutine(MakeRed());
        //if (_currentHealth <= 0f && _isFadingOut)
        //{
        //    StartCoroutine(FadeInDeathScreen(2.0f)); // 1초 동안 서서히 표시
        //    _isFadingOut = false;
        //}
    }
    //private IEnumerator FadeInDeathScreen(float duration)
    //{
    //    float timer = 0f;
    //    Color c = _deathScreen.color;
    //
    //    while (timer < duration)
    //    {
    //        timer += Time.deltaTime;
    //        float alpha = Mathf.Lerp(0f, 1f, timer / duration);
    //        c.a = alpha;
    //        _deathScreen.color = c;
    //        yield return null;
    //    }
    //
    //    c.a = 1f;
    //    _deathScreen.color = c;
    //}
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

    //private IEnumerator MakeRed()
    //{
    //    MaterialPropertyBlock block = new MaterialPropertyBlock();
    //    foreach (Renderer render in _renderers)
    //    {
    //        render.GetPropertyBlock(block);
    //        block.SetColor("_BaseColor", Color.red);
    //        render.SetPropertyBlock(block);
    //    }
    //
    //    yield return new WaitForSeconds(0.1f);
    //
    //    foreach (Renderer render in _renderers)
    //    {
    //        render.GetPropertyBlock(block);
    //        block.SetColor("_BaseColor", Color.white); // 또는 원래 색상 저장했을 경우 그걸로 복원
    //        render.SetPropertyBlock(block);
    //    }
    //}
}
