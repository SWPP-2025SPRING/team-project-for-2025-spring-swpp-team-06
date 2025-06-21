using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleSceneController : MonoBehaviour
{
    // Start is called before the first frame update

    public Button StartButton;
    public Button SettingsButton;
    public Button TutorialButton;
    public Button QuitButton;

    public static string loadTo;

    void Start()
    {
        if (PlayerPrefs.GetFloat("Volume", -1f) < 0)
        {
            // hasn't set volume yet.
            PlayerPrefs.SetFloat("Volume", 0.5f);
        }

        EndingSceneDataHolder.endingSceneInfos = new EndingSceneInfos(-1, -1, -1, "TitleScene");
        loadTo = "Stage1";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("MapSelectionScene");
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnClickSettingsButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene("SettingsScene", LoadSceneMode.Additive);
    }

    public void OnClickTutorialButton()
    {
        SceneManager.LoadScene("Stage0");
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ClearPlayerRefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Volume", 0.5f);
        PlayerPrefs.Save();
    }
}
