using System.Collections;
using UnityEngine;

public class Gunner_Shoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] public Transform firePoint; 
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private RevolverUI revolverUI;
    private int maxAmmo = 6;
    private float reloadTime = 2.0f;
    Gunner_AnimatorController _anim;
    private Gunner_Move _move;
    private int currentAmmo;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        _anim = GetComponent<Gunner_AnimatorController>();
        _move=GetComponent<Gunner_Move>();
        revolverUI = GameObject.Find("Silinder").GetComponent<RevolverUI>();
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetMouseButtonDown(0) && _move.IsAiming)
        {
            if (currentAmmo > 0)
            {
                FireBullet(); // 실제 총알 발사 로직
                revolverUI.Fire(); // UI 회전 및 비활성화
                currentAmmo--;
            }
            else if(Input.GetKeyDown(KeyCode.R))
            {
                isReloading = true;
                //_anim.TriggerReload(); // 상체 트리거 + 무게 1
                StartCoroutine(Reload());
                revolverUI.ReloadAll();
            }
            else
            {
                isReloading = true;

                StartCoroutine(Reload());
                revolverUI.ReloadAll();
            }
        }
    }
    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log(" 장전 시작");
        _anim.TriggerReload();
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        _anim.OnReloadComplete();
        Debug.Log(" 장전 완료");
    }
    private void FireBullet()
    {
        Vector3 dir = firePoint.forward; // 플레이어가 바라보는 정면 방향

        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(dir);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = dir * 20f; // 원하는 속도로 날아가게
        }

        Debug.DrawRay(firePoint.position, dir * 5f, Color.red, 2f); // 시각화 (선택)
    }
}
