using UnityEngine;

public class SetAllBoxCollidersToTrigger : MonoBehaviour
{
    [ContextMenu("Set All BoxColliders to isTrigger = true")]
    void SetTriggers()
    {
        GameObject map = GameObject.Find("Map");
        if (map == null)
        {
            Debug.LogError("Map object not found.");
            return;
        }

        BoxCollider[] boxColliders = map.GetComponentsInChildren<BoxCollider>(true);
        int count = 0;

        foreach (BoxCollider box in boxColliders)
        {
            if (!box.isTrigger)
            {
                box.isTrigger = true;
                count++;
            }
        }

        Debug.Log($"Set isTrigger = true on {count} BoxColliders.");
    }
}
