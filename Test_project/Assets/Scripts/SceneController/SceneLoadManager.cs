using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    private static SceneLoadManager instance;
    public static SceneLoadManager Instance
    {

        // by chatGPT
        get
        {
            if (instance == null)
            {
                // 씬에 존재하지 않으면 직접 생성
                GameObject go = new GameObject("SceneLoadManager");
                instance = go.AddComponent<SceneLoadManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    public string LoadTo { get; set; } = "Stage1";

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
