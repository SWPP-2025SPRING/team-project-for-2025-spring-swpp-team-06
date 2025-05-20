using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MapSelectionButtonController : MonoBehaviour
{

    public string targetSceneName;
    // Start is called before the first frame update
    void Start()
    {
        if(string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Target scene name is not set in the inspector.");
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton(){
        if(!string.IsNullOrEmpty(targetSceneName))
        {
            try
            {
                SceneManager.LoadScene(targetSceneName);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error unloading MapSelectionScene: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Target scene name is not set in the inspector.");
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
