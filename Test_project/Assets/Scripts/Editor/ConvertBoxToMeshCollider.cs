#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ConvertBoxToConvexTriggerMeshCollider : MonoBehaviour
{
    [MenuItem("Tools/Convert Box → Mesh (Convex+Trigger) under Selected")]
    public static void ConvertColliders()
    {
        GameObject root = Selection.activeGameObject;

        if (root == null)
        {
            Debug.LogWarning("❗ 아무 GameObject도 선택되지 않았습니다.");
            return;
        }

        int convertedCount = 0;

        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            BoxCollider box = child.GetComponent<BoxCollider>();
            if (box != null)
            {
                Undo.DestroyObjectImmediate(box); // 히스토리에 남도록 안전하게 제거

                MeshCollider mesh = Undo.AddComponent<MeshCollider>(child.gameObject);
                mesh.convex = true;
                mesh.isTrigger = true;

                convertedCount++;
            }
        }

        Debug.Log($"✅ {convertedCount}개의 BoxCollider를 Convex+Trigger MeshCollider로 변환 완료.");
    }
}
#endif
