using UnityEngine;
using static GameObjectUtils;

public class CoffeeItem : MonoBehaviour
{
    
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        triggered = true;

        OnTriggerActions<CoffeeState>(transform, other, () => new CoffeeState());
    }
    private void OnTriggerStay(Collider other)
    {
        if (triggered) return;
        triggered = true;

        OnTriggerActions<CoffeeState>(transform, other, () => new CoffeeState());
    }
}
