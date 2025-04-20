using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public void OnClick()
    {
        UIManager.Instance.ShowSelectUI();
    }
}
