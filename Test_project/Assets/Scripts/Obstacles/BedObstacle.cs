using System.Collections.Generic;
using UnityEngine;
using static GameObjectUtils;

public class BedObstacle : MonoBehaviour
{
    private HashSet<PlayerControl> playersInBed = new HashSet<PlayerControl>();
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        triggered = true;

        DestroySiblings(transform);

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
    private void OnTriggerStay(Collider other)
    {
        if (triggered) return;
        triggered = true;
        // Destroy once triggered
        
        DestroySiblings(transform);

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
