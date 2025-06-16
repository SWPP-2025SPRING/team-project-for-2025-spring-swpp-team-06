using UnityEngine;

public class AdjustBoxColliders : MonoBehaviour
{
    // 원하는 값
    private static readonly Vector3 newCenter = new Vector3(7.487607e-05f, 0f, -0.007932516f);
    private static readonly Vector3 newSize = new Vector3(0.0003497533f, 0.003080184f, 0.002143905f);

    [ContextMenu("Update All BoxColliders (center & size)")]
    void UpdateBoxColliderDimensions()
    {
        GameObject map = GameObject.Find("Map");
        if (map == null)
        {
            Debug.LogError("Map object not found in scene.");
            return;
        }

        BoxCollider[] boxColliders = map.GetComponentsInChildren<BoxCollider>(true);
        int updatedCount = 0;

        foreach (BoxCollider box in boxColliders)
        {
            box.center = newCenter;
            box.size = newSize;
            updatedCount++;
        }

        Debug.Log($"Updated center and size on {updatedCount} BoxColliders.");
    }
}
