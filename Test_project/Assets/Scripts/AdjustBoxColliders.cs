using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteInEditMode]
public class AdjustBoxColliders : MonoBehaviour
{
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

#if UNITY_EDITOR
            EditorUtility.SetDirty(box);
#endif

            updatedCount++;
        }

#if UNITY_EDITOR
        // 씬 저장
        var scene = map.scene;
        if (scene.IsValid() && scene.isLoaded)
        {
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log($"Saved changes to scene: {scene.name}");
        }
#endif

        Debug.Log($"Updated center and size on {updatedCount} BoxColliders.");
    }
}
