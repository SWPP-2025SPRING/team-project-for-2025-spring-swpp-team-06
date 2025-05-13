using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour
{
    // Start is called before the first frame update

    public Button StartButton;
    public Button SettingsButton;
    public Button TutorialButton;
    public Button QuitButton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStartButton(){
        SceneManager.LoadScene("MapSelectionScene");
    }

    public void OnClickQuitButton(){
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnClickSettingsButton(){
        SceneManager.LoadScene("SettingsScene", LoadSceneMode.Additive);
    }
}
