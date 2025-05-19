using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndingSceneController : MonoBehaviour
{

    private int centisecondInitial = 0;
    public TMP_Text timerText;
    public GameObject GPATextConst;
    public GameObject GPAText;
    public Image GPAGaugeBar;
    public AudioSource audioGaugeDecrease;
    private string timerStringInitial;


    public float animationDuration = 3f;
    public float animationWaitSeconds = 1f;
    // Start is called before the first frame update
    void Start()
    {
        if (InGameUIControl.timer != null)
        {
            centisecondInitial = InGameUIControl.timer.InCentiseconds();
            timerStringInitial = InGameUIControl.timer.ToString();
        }
        else
        {
            Debug.Log("InGameUIControl.timer is null");
            centisecondInitial = 0;
            timerStringInitial = "00:00:00";
        }

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
    // You may use centisecondInitial instead of 6101
    // and targetFillAmount instead of 0.7f
    IEnumerator EndingSequenceTest()
    {
        audioGaugeDecrease.Play();
        StartCoroutine(StopPlayGuageDecrease());
        StartCoroutine(TimerTextAnimation(6101, animationDuration));
        yield return StartCoroutine(GPAGaugeAnimation(0.7f, animationDuration));
        yield return new WaitForSeconds(animationWaitSeconds);

        GPATest();

        yield return new WaitForSeconds(animationWaitSeconds);

        GPATest2();
    }

    public void GPATest()
    {
        GPATextConst.SetActive(true);
    }

    public void GPATest2()
    {
        GPAText.SetActive(true);
    }
}
