using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject testGunnerPrefab;
    //[SerializeField] private GameObject testZombiePrefab;
    MonsterPoolManager _monsterPoolManager;
    private void Awake()
    {
        PlayerStatManager.Instance.LoadAllCharacterData();
        MonsterStatManager.Instance.LoadAllMonsterData();
        _monsterPoolManager = MonsterPoolManager.Instance;
    }
    void Start()
    {
        Vector3 spawnPosition1 = new Vector3(5, 0, 5);
        Vector3 spawnPosition2 = new Vector3(10, 0, 10);

        //MonsterPoolManager.Instance.SpawnMonster(MonsterType.Zombie, 1001, spawnPosition1, Quaternion.identity); // Zombie1
        //MonsterPoolManager.Instance.SpawnMonster(MonsterType.Zombie, 1002, spawnPosition1, Quaternion.identity); // Zombie1

        GameObject prefab = testGunnerPrefab;
        //GameObject prefab2 = testZombiePrefab;
        //GameObject prefab = GameManager.Instance.GetSelectedCharacterPrefab();
        if (prefab != null)
        {
            GameObject player = Instantiate(prefab, new Vector3(0,0,0) , Quaternion.identity);
            SkillSlotManager.Instance.SetPlayer(player);
            //GameObject monster = Instantiate(testZombiePrefab, new Vector3(5,0,5) , Quaternion.identity);

            QuarterViewCamera cam = Camera.main.GetComponent<QuarterViewCamera>();
            if (cam != null)
            {
                cam.SetTarget(player.transform);
            }
        }
    }

}
