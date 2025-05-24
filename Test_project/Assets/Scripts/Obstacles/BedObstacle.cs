using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedObstacle : MonoBehaviour
{
    private HashSet<PlayerControl> playersInBed = new HashSet<PlayerControl>();

    private void OnTriggerEnter(Collider other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        if (player == null) return;

        if (playersInBed.Contains(player)) return;

        foreach (var state in player.States)
        {
            if (state is SleepingState)
                return;
        }

        playersInBed.Add(player);
        player.PushState(new SleepingState());
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        if (player == null) return;

        if (playersInBed.Contains(player))
        {
            playersInBed.Remove(player);
        }
    }
}
