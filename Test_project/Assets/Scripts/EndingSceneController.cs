using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class EndingSceneController : MonoBehaviour
{

    private int centisecondInitial = 0;
    public TMP_Text timerText;
    public Image GPAGaugeBar;
    public AudioSource audioGaugeDecrease;
    public AudioClip audioClipGPAAppearance;
    public AudioClip audioClipYourGPAAppearance;
    public Transform gpaImages;
    private string timerStringInitial;

    public float animationDuration = 3f;
    public float animationWaitSeconds = 1f;

    public int currCentiSecondsTest = 6601;
    public int aPlusCentisecondsTest = 6500;
    public int fCentisecondsTest = 13000;


    public EndingSceneInfos info = EndingSceneDataHolder.endingSceneInfos;

    // Start is called before the first frame update
    void Start()
    {

        if (info == null)
        {
            // Default datas
            Debug.Log("Error: EndingData from the map is null");
            info = new EndingSceneInfos(currCentiSecondsTest, false, 0, aPlusCentisecondsTest, fCentisecondsTest); // Default data
        }

        centisecondInitial = info.GetCurrentScore();
        timerStringInitial = info.GetTimerString();
        // if (InGameUIControl.timer != null)
        // {
        //     centisecondInitial = InGameUIControl.timer.InCentiseconds();
        //     timerStringInitial = InGameUIControl.timer.ToString();
        // }
        // else
        // {
        //     Debug.Log("InGameUIControl.timer is null");
        //     centisecondInitial = 0;
        //     timerStringInitial = "00:00:00";
        // }


        if (centisecondInitial <= 0) centisecondInitial = 0;
        if (timerStringInitial == null) timerStringInitial = "00:00:00";
        if (timerText != null)
        {
            timerText.text = timerStringInitial;
        }

        
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TimerTextAnimation(int startCentiseconds, float duration)
    {
        float elapsed = 0f;
        int endCentiSeconds = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = 1 - Mathf.Pow(1 - t, 3);
            int currentCentiseconds = Mathf.FloorToInt(Mathf.Lerp(startCentiseconds, endCentiSeconds, easedT));
            UpdateTimeText(currentCentiseconds);
            yield return null;
        }

        UpdateTimeText(0);
    }

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

    void UpdateTimeText(int centiseconds)
    {
        int centisecond = centiseconds % 100;
        int minute = centiseconds / 6000;
        int second = (centiseconds - minute * 6000) / 100;

        timerText.text = $"{minute:D2}:{second:D2}:{centisecond:D2}";
    }

    public void test()
    {
        StartCoroutine(EndingSequenceTest());
    }

    // Use this Function to make EndingSequence
    IEnumerator EndingSequenceTest()
    {
        audioGaugeDecrease.Play();
        StartCoroutine(StopPlayGuageDecrease());
        StartCoroutine(TimerTextAnimation(info.GetCurrentScore(), animationDuration));
        yield return StartCoroutine(GPAGaugeAnimation(info.GetFillAmount(), animationDuration));
        
        yield return RevealGPAImage();
    }

    private IEnumerator RevealGPAImage()
    {
        string gpaName = info.GetGPAString();

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
    }
}
