using UnityEngine;
using static GameObjectUtils;

public class SojuObstacle : MonoBehaviour
{
    
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        triggered = true;

        OnTriggerActions<DrunkenState>(transform, other, () => new DrunkenState());
    }
    private void OnTriggerStay(Collider other)
    {
        if (triggered) return;
        triggered = true;

        OnTriggerActions<DrunkenState>(transform, other, () => new DrunkenState());
    }
}

