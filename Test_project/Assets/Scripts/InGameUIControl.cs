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
    public Image speedGauge;
    public List<GameObject> effectsPenalized = new List<GameObject>();

    private float minSpeedScale = 0.106f;
    private float maxSpeedScale = 0.894f;
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

        maybe save game data or do something before exiting

        */

        SceneManager.LoadScene("MapSelectionScene");

        // Scene Load/Unload
    }

    public void OnClickResumeButton(){
        SceneManager.LoadScene("SettingsScene", LoadSceneMode.Additive);
        /*
        
        TODO
        
        */

        // game resume
        // maybe need to give player some time to be ready to resume
        // (give 3 second timer, etc.)

        pausedScene.SetActive(false);
    }

    public void OnClickSettingsButton(){
        if(pausedScene == null) return;
        /*
        
        TODO

        */

        // game settings
    }

    public void OnClickRestartButton()
    {
        /*
        
        TODO
        
        */
        // Things to do before restart(discard score etc.)

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private float PercentToScale(float percent){
        // convert percent to scale
        // 0% -> 0.106
        // 100% -> 0.894
        return minSpeedScale + (maxSpeedScale - minSpeedScale) * percent;
    }

    public void RefreshSpeedGauge(float scale = 0.106f)
    {
        speedGauge.fillAmount = scale;
    }

    public void TogglePenalty(bool isPenalized)
    {
        foreach(GameObject obj in effectsPenalized){
            obj.SetActive(isPenalized);
        }
    }

    public void test1()
    {
        // speed 0%
        RefreshSpeedGauge(minSpeedScale);
    }

    public void test2(){
        // speed 100%
        RefreshSpeedGauge(maxSpeedScale);
    }

    public void test3(){
        // speed 45%
        RefreshSpeedGauge(PercentToScale(0.45f));
    }
    public void test4()
    {
        // penalty on
        TogglePenalty(true);
    }
    public void test5()
    {
        // penalty off
        TogglePenalty(false);
    }
}
