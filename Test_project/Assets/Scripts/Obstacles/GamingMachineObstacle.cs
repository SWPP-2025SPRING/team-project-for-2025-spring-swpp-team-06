using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamingMachineObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        foreach (var state in player.States)
        {
            if (state is PlayerGamingState existingGamingState)
            {
                existingGamingState.ResetTimer();
                return;
            }
        }

        player.PushState(new PlayerGamingState());

    }
}
