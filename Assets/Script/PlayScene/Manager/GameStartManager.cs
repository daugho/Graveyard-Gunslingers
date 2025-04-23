using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject testGunnerPrefab;
    private void Awake()
    {
        PlayerStatManager.Instance.LoadAllCharacterData();
    }
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
}
