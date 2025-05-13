using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUIControl : MonoBehaviour
{
    // Start is called before the first frame update

    public Button menuButton;
    public GameObject pausedScene;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickMenuButton(){
        if(pausedScene == null) return;

        /*
        
        TODO

        */

        // game pause

        pausedScene.SetActive(true);
    }

    // Goto main menu
    public void OnClickExitButton(){
        /*
        
        TODO

        */

        // Go to Main Menu

        // Scene Load/Unload
    }

    public void OnClickResumeButton(){
        if(pausedScene == null) return;
        /*
        
        TODO
        
        */

        // game resume
        // maybe need to give player some time to be ready to resume
        // (give 3 second timer, etc.)

        pausedScene.SetActive(false);
    }
}
