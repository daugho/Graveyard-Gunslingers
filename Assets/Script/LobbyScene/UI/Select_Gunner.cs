using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Select_Gunner : MonoBehaviour
{
    [SerializeField]
    private Button _selectGunnerButton;
    void Start()
    {
        _selectGunnerButton.onClick.AddListener(OnClickSelectGunner);
    }

    private void OnClickSelectGunner()
    {
        
        StartCoroutine(LoadSceneAsync("PlayScene"));
    }
    private IEnumerator LoadSceneAsync(string scene)
    {
        UIManager.Instance.PlayUI();
        yield return SceneManager.LoadSceneAsync(scene);
    }
}
