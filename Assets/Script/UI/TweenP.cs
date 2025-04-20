using UnityEngine;
using uTools;

public class TweenP : MonoBehaviour
{
    [SerializeField]
    uTweenPosition _tween;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var tween = uTweenPosition.Begin(_tween.gameObject,new Vector3(-1200,0,0),new Vector3(0,0,0));
            tween.onFinished.AddListener(ShowLog);
        }
    }
    private void ShowLog()
    {
        Debug.Log("End Log");
    }
}
