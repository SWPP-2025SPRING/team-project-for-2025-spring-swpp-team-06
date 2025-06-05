using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsSceneController : MonoBehaviour
{

    public Slider sliderVolume;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetFloat("Volume", -1f) < 0)
        {
            // hasn't set volume yet.
            PlayerPrefs.SetFloat("Volume", 0.5f);
        }

        sliderVolume.value = PlayerPrefs.GetFloat("Volume", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickBackButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.UnloadSceneAsync("SettingsScene");
    }

    public void ChangeVolume()
    {
        PlayerPrefs.SetFloat("Volume", sliderVolume.value);
    }

}
