using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EndingSceneController : MonoBehaviour
{

    private int centisecondInitial = 0;
    public TMP_Text timerText;
    public Image GPAGaugeBar;
    public AudioSource audioGaugeDecrease;
    public AudioClip audioClipGPAAppearance;
    public AudioClip audioClipYourGPAAppearance;
    public Transform gpaImages;
    public GameObject buttons;
    private string timerStringInitial;

    public float animationDuration = 3f;
    public float animationWaitSeconds = 1f;

    public int currCentiSecondsTest = 6601;
    public int aPlusCentisecondsTest = 6500;
    public int fCentisecondsTest = 13000;

    private const int mapCount = 6;


    // Start is called before the first frame update
    void Start()
    {

        if (EndingSceneDataHolder.endingSceneInfos == null || EndingSceneDataHolder.endingSceneInfos.GetCurrentScore() == -1)
        {
            // endingSceneInfo Not properly set. -1 is Initial state
            // Default datas
            Debug.Log("Error: EndingData from the map is null");
            EndingSceneDataHolder.endingSceneInfos.SetInfos(currCentiSecondsTest, false, 0, aPlusCentisecondsTest, fCentisecondsTest, "null"); // Default data
        }

        centisecondInitial = EndingSceneDataHolder.endingSceneInfos.GetCurrentScore();
        timerStringInitial = EndingSceneDataHolder.endingSceneInfos.GetTimerString();


        if (centisecondInitial <= 0) centisecondInitial = 0;
        if (timerStringInitial == null) timerStringInitial = "00:00:00";
        if (timerText != null)
        {
            timerText.text = timerStringInitial;
        }

        StartCoroutine(StartWithDelay());

    }

    IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(1f);  // 1초 기다리기
        yield return StartCoroutine(EndingSequence());  // ▶실제 코루틴 실행
    }

    // Update is called once per frame
    void Update()
    {

    }

    // IEnumerator TimerTextAnimation(int startCentiseconds, float duration)
    // {
    //     float elapsed = 0f;
    //     int endCentiSeconds = 0;

    //     while (elapsed < duration)
    //     {
    //         elapsed += Time.deltaTime;
    //         float t = Mathf.Clamp01(elapsed / duration);
    //         float easedT = 1 - Mathf.Pow(1 - t, 3);
    //         int currentCentiseconds = Mathf.FloorToInt(Mathf.Lerp(startCentiseconds, endCentiSeconds, easedT));
    //         UpdateTimeText(currentCentiseconds);
    //         yield return null;
    //     }

    //     UpdateTimeText(0);
    // }

    IEnumerator GPAGaugeAnimation(float endPointFillAmount, float duration)
    {
        float elapsed = 0f;
        float initialFillAmount = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = 1 - Mathf.Pow(1 - t, 3);
            float fill = (float)Mathf.Lerp(initialFillAmount, endPointFillAmount, easedT);
            GPAGaugeBar.fillAmount = fill;
            yield return null;
        }
    }

    IEnumerator StopPlayGuageDecrease()
    {
        yield return new WaitForSeconds(animationDuration);
        audioGaugeDecrease.Stop();
    }

    // void UpdateTimeText(int centiseconds)
    // {
    //     int centisecond = centiseconds % 100;
    //     int minute = centiseconds / 6000;
    //     int second = (centiseconds - minute * 6000) / 100;

    //     timerText.text = $"{minute:D2}:{second:D2}:{centisecond:D2}";
    // }
    // Use this Function to make EndingSequence
    IEnumerator EndingSequence()
    {
        audioGaugeDecrease.Play();
        StartCoroutine(StopPlayGuageDecrease());
        //StartCoroutine(TimerTextAnimation(EndingSceneDataHolder.endingSceneInfos.GetCurrentScore(), animationDuration));
        yield return StartCoroutine(GPAGaugeAnimation(EndingSceneDataHolder.endingSceneInfos.GetFillAmount(), animationDuration));

        yield return RevealGPAImage();
    }

    private IEnumerator RevealGPAImage()
    {
        string gpaName = EndingSceneDataHolder.endingSceneInfos.GetGPAString();

        yield return new WaitForSeconds(animationWaitSeconds);
        audioGaugeDecrease.PlayOneShot(audioClipGPAAppearance);
        foreach (Transform child in gpaImages)
        {
            if (child.name == gpaName)
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
        yield return new WaitForSeconds(animationWaitSeconds);
        buttons.SetActive(true);
    }

    public void OnClickMainMenuButton()
    {
        if ((int)EndingSceneDataHolder.endingSceneInfos.GetGPA() <= mapCount - 1
        && EndingSceneDataHolder.endingSceneInfos.GetMapIndex() <= mapCount - 1
        && !EndingSceneDataHolder.endingSceneInfos.IsTutorial())
        {
            // if above B+ and this map is not the last one
            // should open next stage
            /*
            
            TODO

            open next stage
            animation or effect when opening map(deliver this info to MapSelectionScene)
            
            */
            int mapToUnlock = EndingSceneDataHolder.endingSceneInfos.GetMapIndex() + 1;
            PlayerPrefs.SetInt("NewMapToUnlock", mapToUnlock);
            PlayerPrefs.SetInt("ShouldUnlockNewMap", 1);
            PlayerPrefs.Save();
            //Debug.Log(PlayerPrefs.GetInt("ShouldUnlockNewMap", -1));

            StartCoroutine(LoadMainFrame());

        }
        else
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    public void OnClickMapSelectionButton()
    {
        if ((int)EndingSceneDataHolder.endingSceneInfos.GetGPA() <= mapCount - 1
        && EndingSceneDataHolder.endingSceneInfos.GetMapIndex() <= mapCount - 1
        && !EndingSceneDataHolder.endingSceneInfos.IsTutorial())
        {
            // if above B+ and this map is not the last one
            // should open next stage
            /*
            
            TODO

            open next stage
            animation or effect when opening map(deliver this info to MapSelectionScene)
            
            */
            int mapToUnlock = EndingSceneDataHolder.endingSceneInfos.GetMapIndex() + 1;
            PlayerPrefs.SetInt("NewMapToUnlock", mapToUnlock);
            PlayerPrefs.SetInt("ShouldUnlockNewMap", 1);
            PlayerPrefs.Save();
            //Debug.Log(PlayerPrefs.GetInt("ShouldUnlockNewMap", -1));

            StartCoroutine(LoadMapSelectionNextFrame());

        }
        else
        {
            SceneManager.LoadScene("MapSelectionScene");
        }
    }

    IEnumerator LoadMapSelectionNextFrame()
    {
        yield return null; // 한 프레임 대기
        SceneManager.LoadScene("MapSelectionScene");
    }

    IEnumerator LoadMainFrame()
    {
        yield return null; // 한 프레임 대기
        SceneManager.LoadScene("TitleScene");
    }

    public void OnClickRestartButton()
    {
        SceneManager.LoadScene(EndingSceneDataHolder.endingSceneInfos.GetMapName());
    }
}
