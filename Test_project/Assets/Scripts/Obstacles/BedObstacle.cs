using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedObstacle : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
        {
            return;
        }

        PlayerControl player = other.GetComponent<PlayerControl>();
        if (player == null)
        {
            return;
        }

        hasTriggered = true;
        player.ChangeState(new SleepingState());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerControl>() != null)
        {
            hasTriggered = false;
        }
    }

}
