using UnityEngine;
using Mirror;

public class ScoreBarFacesCamera : MonoBehaviour
{
    //LateUpdate чтобы все обновления камеры были завершены
    
    void TurnScoreBarToClient ()
    {
        transform.forward = Camera.main.transform.forward;
    }

    void LateUpdate()
    {
        TurnScoreBarToClient();
    }
}
