using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SojuObstacle : MonoBehaviour
{
    
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {

        if (triggered) return;
        triggered = true;
        // Destroy once triggered
        Transform parent = transform.parent;
        if (parent.gameObject.CompareTag("Ground")) return;

        foreach (Transform child in parent)
        {
            // Destroy all objects in same group
            Destroy(child.gameObject);
        }

        PlayerControl player = other.GetComponent<PlayerControl>();
        foreach (var state in player.States)
        {
            if (state is DrunkenState existingGamingState)
            {
                existingGamingState.ResetTimer();
                return;
            }
        }

        player.PushState(new DrunkenState());



    }
    private void OnTriggerStay(Collider other)
    {

        if (triggered) return;
        triggered = true;
        // Destroy once triggered
        Transform parent = transform.parent;
        if (parent.gameObject.CompareTag("Ground")) return;

        foreach (Transform child in parent)
        {
            // Destroy all objects in same group
            Destroy(child.gameObject);
        }

        PlayerControl player = other.GetComponent<PlayerControl>();
        foreach (var state in player.States)
        {
            if (state is DrunkenState existingGamingState)
            {
                existingGamingState.ResetTimer();
                return;
            }
        }

        player.PushState(new DrunkenState());



    }
}

