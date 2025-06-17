using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedObstacle : MonoBehaviour
{
    private HashSet<PlayerControl> playersInBed = new HashSet<PlayerControl>();
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        triggered = true;
        Debug.Log(transform.position.z);
        // Destroy once triggered
        Transform parent = transform.parent;
        if (parent.gameObject.CompareTag("Ground")) return; // If it is directly in ground

        foreach (Transform child in parent)
        {
            // Destroy all objects in same group
            Destroy(child.gameObject);
        }

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
        Debug.Log(transform.position.z);
        // Destroy once triggered
        Transform parent = transform.parent;
        if (parent.gameObject.CompareTag("Ground")) return; // If it is directly in ground

        foreach (Transform child in parent)
        {
            // Destroy all objects in same group
            Destroy(child.gameObject);
        }

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
