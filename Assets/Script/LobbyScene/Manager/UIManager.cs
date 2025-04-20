using UnityEngine;

public class UIManager : MonoBehaviour
{    public static UIManager Instance { get; private set; }

    public GameObject _lobbyUI;
    public GameObject _selectUI;
    //public GameObject _playUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private UIManager() { }
    public void ShowSelectUI()
    {
        _lobbyUI.SetActive(false);
        _selectUI.SetActive(true);
        //_playUI.SetActive(false);
    }

    public void ShowLobbyUI()
    {
        _selectUI.SetActive(false);
        _lobbyUI.SetActive(true);
        //_playUI.SetActive(false);
    }
    public void PlayUI()
    {
        _selectUI.SetActive(false);
        _lobbyUI.SetActive(false);
        //_playUI.SetActive(true);
    }

}
