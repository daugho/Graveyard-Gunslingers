using UnityEngine;

public class NextRoundButton : MonoBehaviour
{   
    public void OnClickNextRound()
    {
        RoundManager.Instance?.StartNextRound();
    }
}
