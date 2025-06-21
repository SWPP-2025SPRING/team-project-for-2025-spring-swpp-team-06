using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEditor;

public class TutorialEndingSceneControl : MonoBehaviour
{
    public GameObject buttons;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickMainMenuButton()
    {
        SceneManager.LoadScene("TitleScene");

    }

    public void OnClickMapSelectionButton()
    {
        SceneManager.LoadScene("MapSelectionScene");
    }
}
