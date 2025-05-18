using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        if (player.GetCurrentState() is CoffeeState currentGamingState)
        {
            currentGamingState.ResetTimer();
        }
        else
        {
            player.ChangeState(new CoffeeState());
        }
    }
}
