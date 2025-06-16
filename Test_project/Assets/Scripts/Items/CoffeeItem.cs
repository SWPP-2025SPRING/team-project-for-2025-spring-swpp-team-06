using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeItem : MonoBehaviour
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
            Destroy(child.gameObject);
        }
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
    private void OnTriggerStay(Collider other)
    {

        if (triggered) return;
        triggered = true;

        // Destroy once triggered
        Transform parent = transform.parent;
        if (parent.gameObject.CompareTag("Ground")) return;

        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
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
