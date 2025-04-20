using UnityEngine;

public class UIManager : MonoBehaviour
{    public static UIManager Instance { get; private set; }

    public GameObject lobbyUI;
    public GameObject selectUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private UIManager() { }
    public void ShowSelectUI()
    {
        lobbyUI.SetActive(false);
        selectUI.SetActive(true);
    }

    public void ShowLobbyUI()
    {
        selectUI.SetActive(false);
        lobbyUI.SetActive(true);
    }

}
