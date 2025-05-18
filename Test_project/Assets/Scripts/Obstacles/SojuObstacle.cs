using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SojuObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        if (player.GetCurrentState() is DrunkenState currentGamingState)
        {
            currentGamingState.ResetTimer();
        }
        else
        {
            player.ChangeState(new DrunkenState());
        }
    }
}
