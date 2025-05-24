using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        foreach (var state in player.States)
        {
            if (state is CoffeeState existingGamingState)
            {
                existingGamingState.ResetTimer();
                return;
            }
        }

        player.PushState(new CoffeeState());

    }
}
