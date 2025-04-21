using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject testGunnerPrefab;
    void Start()
    {

        GameObject prefab = testGunnerPrefab;
        //GameObject prefab = GameManager.Instance.GetSelectedCharacterPrefab();
        if (prefab != null)
        {
            GameObject player = Instantiate(prefab, new Vector3(0,0,0) , Quaternion.identity);

            QuarterViewCamera cam = Camera.main.GetComponent<QuarterViewCamera>();
            if (cam != null)
            {
                cam.SetTarget(player.transform);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
