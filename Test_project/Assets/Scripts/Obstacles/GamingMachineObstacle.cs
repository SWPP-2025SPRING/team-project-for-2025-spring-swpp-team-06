using UnityEngine;
using static GameObjectUtils;

public class GamingMachineObstacle : MonoBehaviour
{
    
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        triggered = true;

        OnTriggerActions<PlayerGamingState>(transform, other, () => new PlayerGamingState());
    }
    private void OnTriggerStay(Collider other)
    {
        if (triggered) return;
        triggered = true;

        OnTriggerActions<PlayerGamingState>(transform, other, () => new PlayerGamingState());
    }
}
