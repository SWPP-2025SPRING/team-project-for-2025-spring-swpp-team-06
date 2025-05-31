using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


        // Destroy once triggered
        Transform parent = transform.parent;
        if (parent.gameObject.CompareTag("Ground")) return;

        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
