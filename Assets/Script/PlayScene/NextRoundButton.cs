using UnityEngine;

public class NextRoundButton : MonoBehaviour
{   
    bool isActive = false;

    public void OnClickNextRound()
    {
        RoundManager.Instance?.StartNextRound();
    }
}
