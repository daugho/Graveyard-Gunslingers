using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            _pool.Enqueue(bullet);
        }
    }
    public GameObject GetBullet()
    {
        
       
            GameObject bullet = _pool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        

        // 풀 크기 초과 시 예외 처리 또는 추가 생성
        //GameObject extraBullet = Instantiate(bulletPrefab);
        //return extraBullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        _pool.Enqueue(bullet);
    }
}