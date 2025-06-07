using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using TMPro;

public class MapSelectionButtonController : MonoBehaviour
{

    public string targetSceneName;
    public TMP_Text bestTimeText;
    public TMP_Text gpaText;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        string objName = gameObject.name;
        index = GetIndexFromName(objName);
        System.Diagnostics.Debug.Assert(index > 0);

        int centiseconds = PlayerPrefs.GetInt("Best" + index, -1);
        GPA gpa = (GPA)PlayerPrefs.GetInt("GPA" + index, -1);

        if (centiseconds >= 0)
        {
            Debug.Assert((int)gpa >= 0);
            //played before and has GPA
            SetTexts(centiseconds, gpa);
        }
        
        if (string.IsNullOrEmpty(targetSceneName))
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

    private int GetIndexFromName(string name)
    {
        Match match = Regex.Match(name, @"\d+");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        else return 0;
    }

    private void SetTexts(int centiseconds, GPA gpa)
    {
        bestTimeText.text = "Best Time: " + CentisecondsToString(centiseconds);
        gpaText.text = "GPA: " + GPAToString(gpa);
    }
    
    public string CentisecondsToString(int centiseconds_)
    {
        int naiveMinute = centiseconds_ / 6000;
        int minute_ = naiveMinute > 60 ? 60 : centiseconds_ / 6000;
        int second_ = (centiseconds_ - naiveMinute * 6000) / 100 > 60 ? 60 : (centiseconds_ - naiveMinute * 6000) / 100;
        int centisecond_ = centiseconds_ % 100 > 100 ? 100 : centiseconds_ % 100;
        return $"{minute_:D2}:{second_:D2}:{centisecond_:D2}";
    }

    private string GPAToString(GPA gpa) {
        switch (gpa)
        {
            case GPA.Aplus:
                return "A+";
            case GPA.Azero:
                return "A0";
            case GPA.Aminus:
                return "A-";
            case GPA.Bplus:
                return "B+";
            case GPA.Bzero:
                return "B0";
            case GPA.Bminus:
                return "B-";
            case GPA.Cplus:
                return "C+";
            case GPA.Czero:
                return "C0";
            case GPA.Cminus:
                return "C-";
            case GPA.Dplus:
                return "D+";
            case GPA.Dzero:
                return "D0";
            case GPA.Dminus:
                return "D-";
            default:
                return "F";
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
                TitleSceneController.loadTo = targetSceneName;
                SceneManager.LoadScene("Loading");
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
