using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gunnerPrefab;
    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject magePrefab;

    public CharacterType SelectedCharacter { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SetSelectedCharacter(CharacterType type)
    {
        SelectedCharacter = type;
    }

    public GameObject GetSelectedCharacterPrefab()
    {
        return SelectedCharacter switch
        {
            CharacterType.Gunner => gunnerPrefab,
            CharacterType.Knight => knightPrefab,
            CharacterType.Mage => magePrefab,
            _ => null
        };
    }
}
