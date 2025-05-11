using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BulletType
{
    Normal,
    Penetration,
    Explosive,
    Lightning,
}

public class Gunner_Shoot : MonoBehaviour
{
    [Header(" 탄별 프리팬 설정")]
    [SerializeField] private GameObject normalBulletPrefab;
    [SerializeField] private GameObject penetrationBulletPrefab;
    [SerializeField] private GameObject explosionBUlletPrefab;
    [SerializeField] private GameObject lightningBulletPrefab;

    [SerializeField] public Transform firePoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private RevolverUI revolverUI;

    private Dictionary<BulletType, GameObject> bulletPrefabs;

    private int _maxAmmo = 6;
    private float _reloadTime = 2.0f;
    private int _currentAmmo;
    private bool _isReloading = false;

    private BulletType _currentBulletType = BulletType.Normal;
    private SkillData _loadedSkill = null;
    private int _specialAmmoLeft = 0;

    private float _playerDamage;
    private Gunner_AnimatorController _anim;
    private Gunner_Move _move;
    private Player_Gunner _playerGunner;

    void Start()
    {
        _currentAmmo = _maxAmmo;
        _anim = GetComponent<Gunner_AnimatorController>();
        _move = GetComponent<Gunner_Move>();
        revolverUI = GameObject.Find("Silinder").GetComponent<RevolverUI>();
        _playerGunner = GetComponent<Player_Gunner>();
        _playerDamage = _playerGunner.Stats.GetDamage();

        bulletPrefabs = new Dictionary<BulletType, GameObject>
        {
            { BulletType.Normal, normalBulletPrefab },
            { BulletType.Penetration, penetrationBulletPrefab },
            { BulletType.Explosive, explosionBUlletPrefab },
            { BulletType.Lightning, lightningBulletPrefab }
        };
    }

    void Update()
    {
        if (_isReloading)
            return;

        if (Input.GetMouseButtonDown(0) && _move.IsAiming)
        {
            if (_currentAmmo > 0)
            {
                FireBullet();
                revolverUI.Fire();
                _currentAmmo--;

                if (_currentBulletType != BulletType.Normal)
                {
                    _specialAmmoLeft--;
                    if (_specialAmmoLeft <= 0)
                    {
                        _currentBulletType = BulletType.Normal;
                        _loadedSkill = null;
                        Debug.Log("[총] 특수탄 소진 → 일반탄 복귀");
                    }
                }
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        Debug.Log("장전 시작");
        _anim.TriggerReload();
        yield return new WaitForSeconds(_reloadTime);
        _currentAmmo = _maxAmmo;
        _isReloading = false;
        _anim.OnReloadComplete();
        revolverUI.ReloadAll();
        Debug.Log("장전 완료");
    }

    private void FireBullet()
    {
        Vector3 dir = firePoint.forward;
        GameObject prefab = bulletPrefabs[_currentBulletType];
        GameObject bullet = Instantiate(prefab, firePoint.position, Quaternion.LookRotation(dir));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = dir * 20f;

        switch (_currentBulletType)
        {
            case BulletType.Normal:
                bullet.GetComponent<Bullet>()?.SetDamage(_playerDamage);
                break;
            case BulletType.Penetration:
                var pb = bullet.GetComponent<PenetrationBullet>();
                if (pb != null && _loadedSkill != null)
                {
                    pb.Initialize(_playerDamage, _loadedSkill);
                }
                break;
            case BulletType.Explosive:
                var eb = bullet.GetComponent<ExplosiveBullet>();
                if (eb != null && _loadedSkill != null)
                {
                    eb.Initialize(_playerDamage, _loadedSkill);
                }
                break;
            case BulletType.Lightning:
                var lb = bullet.GetComponent<ChainLightningBullet>();
                if (lb != null && _loadedSkill != null)
                {
                    lb.Initialize(_playerDamage, _loadedSkill);
                }
                break;

        }

        Debug.DrawRay(firePoint.position, dir * 5f, Color.red, 2f);
    }

    public void LoadSkillBullet(SkillData skill)
    {
        _loadedSkill = skill;

        if (skill.Name == "Pb")
            _currentBulletType = BulletType.Penetration;
        else if (skill.Name == "Eb")
            _currentBulletType = BulletType.Explosive;
        else if (skill.Name == "Lb")
            _currentBulletType = BulletType.Lightning;
        else
            _currentBulletType = BulletType.Normal;

        _specialAmmoLeft = _maxAmmo;
        _currentAmmo = _maxAmmo;

        revolverUI.ReloadAll();
        Debug.Log($"[스킬] {skill.Name} 장전됨 → {_currentBulletType} / {_specialAmmoLeft}발");
    }

    public void UpdateDamage(float newDamage)
    {
        _playerDamage = newDamage;
    }
}


//using System.Collections;
//using UnityEngine;
//using static StatManager;
//
//public class Gunner_Shoot : MonoBehaviour
//{
//    [SerializeField] private GameObject bulletPrefab;
//    [SerializeField] public Transform firePoint; 
//    [SerializeField] private LayerMask groundLayer;
//    [SerializeField] private RevolverUI revolverUI;
//    private int maxAmmo = 6;
//    private float reloadTime = 2.0f;
//    Gunner_AnimatorController _anim;
//    private Gunner_Move _move;
//    private int currentAmmo;
//    private bool isReloading = false;
//    private float _playerDamage;
//    Player_Gunner _playerGunner;
//
//    void Start()
//    {
//        currentAmmo = maxAmmo;
//        _anim = GetComponent<Gunner_AnimatorController>();
//        _move=GetComponent<Gunner_Move>();
//        revolverUI = GameObject.Find("Silinder").GetComponent<RevolverUI>();
//        _playerGunner = GetComponent<Player_Gunner>();
//        _playerDamage = _playerGunner.Stats.GetDamage();
//    }
//
//    void Update()
//    {
//        if (isReloading)
//            return;
//
//        if (Input.GetMouseButtonDown(0) && _move.IsAiming)
//        {
//            if (currentAmmo > 0)
//            {
//                FireBullet(); // 실제 총알 발사 로직
//                revolverUI.Fire(); // UI 회전 및 비활성화
//                currentAmmo--;
//            }
//            else if(Input.GetKeyDown(KeyCode.R))
//            {
//                isReloading = true;
//                //_anim.TriggerReload(); // 상체 트리거 + 무게 1
//                StartCoroutine(Reload());
//                revolverUI.ReloadAll();
//            }
//            else
//            {
//                isReloading = true;
//
//                StartCoroutine(Reload());
//                revolverUI.ReloadAll();
//            }
//        }
//    }
//    private IEnumerator Reload()
//    {
//        isReloading = true;
//        Debug.Log(" 장전 시작");
//        _anim.TriggerReload();
//        yield return new WaitForSeconds(reloadTime);
//
//        currentAmmo = maxAmmo;
//        isReloading = false;
//        _anim.OnReloadComplete();
//        Debug.Log(" 장전 완료");
//    }
//    private void FireBullet()
//    {
//        Vector3 dir = firePoint.forward; // 플레이어가 바라보는 정면 방향
//
//        GameObject bullet = BulletPool.Instance.GetBullet();
//        bullet.transform.position = firePoint.position;
//        bullet.transform.rotation = Quaternion.LookRotation(dir);
//
//        Rigidbody rb = bullet.GetComponent<Rigidbody>();
//        if (rb != null)
//        {
//            rb.linearVelocity = dir * 20f; // 원하는 속도로 날아가게
//        }
//        Bullet bulletScript = bullet.GetComponent<Bullet>();
//        if (bulletScript != null)
//        {
//            bulletScript.SetDamage(_playerDamage); // ⭐️ 그냥 저장된 값 사용
//        }
//
//        Debug.DrawRay(firePoint.position, dir * 5f, Color.red, 2f); // 시각화 (선택)
//    }
//    public void UpdateDamage(float newDamage)
//    {
//        _playerDamage = newDamage;
//    }
//}
