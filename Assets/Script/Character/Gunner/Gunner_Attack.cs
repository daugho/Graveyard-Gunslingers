using UnityEngine;

public class Gunner_Attack : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _bulletSpeed = 20f;
    private Gunner_Move _move;
    void Start()
    {
        _move = GetComponent<Gunner_Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_move == null || !_move.IsAiming)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
    private void Fire()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * _bulletSpeed;

    }
}
