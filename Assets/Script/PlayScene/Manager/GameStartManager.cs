using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    void Start()
    {
        GameObject prefab = GameManager.Instance.GetSelectedCharacterPrefab();
        if (prefab != null)
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
