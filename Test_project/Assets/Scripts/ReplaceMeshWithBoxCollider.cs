using UnityEngine;

public class ReplaceMeshWithBoxCollider : MonoBehaviour
{
    // target BoxCollider parameters
    private static readonly Vector3 boxCenter = new Vector3(0f, 0f, -0.007932512f);
    private static readonly Vector3 boxSize = new Vector3(0.0002000006f, 0.003080184f, 0.002143903f);

    [ContextMenu("Replace All MeshColliders With BoxColliders")]
    void ReplaceColliders()
    {
        GameObject map = GameObject.Find("Map");
        if (map == null)
        {
            Debug.LogError("Map object not found in scene.");
            return;
        }

        MeshCollider[] meshColliders = map.GetComponentsInChildren<MeshCollider>(true);
        int replacedCount = 0;

        foreach (MeshCollider mc in meshColliders)
        {
            GameObject obj = mc.gameObject;
            DestroyImmediate(mc); // Remove MeshCollider immediately in Editor

            BoxCollider box = obj.AddComponent<BoxCollider>();
            box.center = boxCenter;
            box.size = boxSize;

            replacedCount++;
        }

        Debug.Log($"Replaced {replacedCount} MeshColliders with BoxColliders.");
    }
}
