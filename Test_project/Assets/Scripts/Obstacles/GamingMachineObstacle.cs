using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamingMachineObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        if (player.GetCurrentState() is PlayerGamingState currentGamingState)
        {
            currentGamingState.ResetTimer();
        }
        else
        {
            player.ChangeState(new PlayerGamingState());
        }
    }
}
