using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;


public class InGameUIControl : MonoBehaviour
{
    // Start is called before the first frame update

    public Button menuButton;
    public GameObject pausedScene;
    public Image speedGauge;
    public List<GameObject> effectsPenalized = new List<GameObject>();
    public List<GameObject> effectsBuffed = new List<GameObject>();
    public static Timer timer;
    public GameObject player;
    public static InGameUIControl instance;
    public TMP_Text timerText;
    public TMP_Text speedText;
    public TMP_Text stageText;
    private PlayerControl playerControlScript;

    public Image bedTimeGauge;
    public Image coffeeTimeGauge;
    public Image gamingTimeGauge;
    public Image sojuTimeGauge;
    public Image energyTimeGauge;

    private float maxSpeedScale = 0.67f;

    private float maxSpeed = 40f;
    private bool isStartTextDestroyed = false;
    private bool isGameStarted = false;
    public static bool isMenuPopped = false;
    private float elapsedTime = 0f;
    private bool isTutorial = false, isSetting = false;

    private Dictionary<Image, Coroutine> gaugeCoroutines = new Dictionary<Image, Coroutine>();


    void Awake()
    {
        timer = new Timer(0);
        speedText.text = "0";
        RefreshSpeedGauge(0f);
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogError("No Player object in this map");
            }
        }

        playerControlScript = player?.GetComponent<PlayerControl>();
        if (playerControlScript == null) Debug.LogError("No PlayerControl script");

        string currentSceneName = SceneManager.GetActiveScene().name;
        int mapIndex = GetMapIndex(currentSceneName);
        if (mapIndex == 0) {
            stageText.text = "Tutorial";
            isTutorial = true;
            isStartTextDestroyed = true;
            isGameStarted = true;
            ToggleStartTexts(false);
            playerControlScript.isTutorial = true;
            Debug.Log("Tutorial!");
        }
        else {
            Debug.Assert(mapIndex > 0);
            stageText.text = $"Stage {mapIndex}";
        }
        if (SceneLoadManager.Instance.LoadTo == null) SceneLoadManager.Instance.LoadTo = "TitleScene";

    }

    public void TurnOffGauge(Image gauge)
    {
        gauge.gameObject.SetActive(false);
    }

    public void TurnOnGauge(Image gauge)
    {
        gauge.gameObject.SetActive(true);
        gauge.fillAmount = 1f;
    }

    public void InitiateGauge(float duration, Image img)
    {
        TurnOnGauge(img);

        //by chatGPT
        if (gaugeCoroutines.TryGetValue(img, out Coroutine existingCoroutine))
        {
            // 이미 같은 페널티가 적용 중이었을 경우
            StopCoroutine(existingCoroutine);
        }

        Coroutine newCoroutine = StartCoroutine(DecreaseGauge(duration, img));
        gaugeCoroutines[img] = newCoroutine;

    }

    public IEnumerator DecreaseGauge(float duration, Image img)
    {
        float elapsed = 0f;
        float startFill = 1f;
        float endFill = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            img.fillAmount = Mathf.Lerp(startFill, endFill, t);
            yield return null;
        }

        TurnOffGauge(img);
        gaugeCoroutines.Remove(img);
    } 

    void Start()
    {
        Application.targetFrameRate = 60; // 원하는 FPS로 설정 (예: 60FPS) 
    }

    // Update is called once per frame
    void Update()
    {
        if (isMenuPopped || isSetting) return;

        if (Input.GetKeyDown(KeyCode.Escape)){
            if(isMenuPopped) OnClickResumeButton();
            else OnClickMenuButton();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && !isStartTextDestroyed && !isTutorial)
        {
            isStartTextDestroyed = true;
            isGameStarted = true;
            ToggleStartTexts(false);
        }

        if (isGameStarted)
        {
            // timer adjust
            elapsedTime += Time.deltaTime;

            int centiseconds = Mathf.FloorToInt(elapsedTime * 100);
            timer.UpdateTimer(centiseconds);
            timerText.text = timer.ToString();

            // speed adjust
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            float speedf = playerRb.velocity.magnitude;
            int speed = Mathf.RoundToInt(speedf);
            speedText.text = speed.ToString();

            // speed gauge adjust
            RefreshSpeedGauge(speedf);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnClickRestartButton();
        }
    }

    public void ToggleStartTexts(bool trigger)
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child.CompareTag("DestroyOnStart"))
            {
                child.gameObject.SetActive(trigger);
            }
        }
    }

    public int GetMapIndex(string mapName)
    {
        Match match = Regex.Match(mapName, @"\d+$");

        if (match.Success)
        {
        return int.Parse(match.Value);
        }
        return -1;
    }

    public void OnClickMenuButton()
    {
        if (pausedScene == null) return;

        isMenuPopped = true;

        Time.timeScale = 0f;
        if (playerControlScript != null)
        {
            playerControlScript.isPaused = true;
        }

        pausedScene.SetActive(true);
    }

    // Goto main menu
    public void OnClickExitToMainButton()
    {
        RefreshTimeScale();

        isMenuPopped = false;
        if(EndingSceneDataHolder.endingSceneInfos != null) EndingSceneDataHolder.endingSceneInfos.SetInfos(-1, -1, -1, "TitleScene");
        SceneLoadManager.Instance.LoadTo = "TitleScene";
        SceneManager.LoadScene("Loading");

    }

    public void RefreshTimeScale(){
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f; // reset time scale before exiting
        }
    }

    // Goto Map Selection
    public void OnClickExitToMapSelectionButton()
    {
        RefreshTimeScale();

        isMenuPopped = false;

        SceneLoadManager.Instance.LoadTo = "MapSelectionScene";
        SceneManager.LoadScene("Loading");

    }

    public void OnClickResumeButton()
    {
        if(!isTutorial) ToggleStartTexts(true);

        isMenuPopped = false;

        pausedScene.SetActive(false);
    }

    public void OnClickSettingsButton()
    {
        if (pausedScene == null) return;
        
        SceneManager.LoadScene("SettingsScene", LoadSceneMode.Additive);
    }

    public void OnClickRestartButton()
    {
        isMenuPopped = false;

        Scene currentScene = SceneManager.GetActiveScene();
        
        SceneLoadManager.Instance.LoadTo = currentScene.name;
        SceneManager.LoadScene("Loading");
    }

    public void RefreshSpeedGauge(float speed = 0.0f)
    {
        float scale = speed / maxSpeed * maxSpeedScale;
        speedGauge.fillAmount = scale;
    }

    public void TogglePenalty(bool isPenalized)
    {
        foreach (GameObject obj in effectsPenalized)
        {
            obj.SetActive(isPenalized);
        }
    }

    public void ToggleBuff(bool isBuffed)
    {
        foreach (GameObject obj in effectsBuffed)
        {
            obj.SetActive(isBuffed);
        }
    }

    // public void test1()
    // {
    //     // speed 0%
    //     RefreshSpeedGauge(minSpeedScale);
    // }

    // public void test2()
    // {
    //     // speed 100%
    //     RefreshSpeedGauge(maxSpeedScale);
    // }

    // public void test3()
    // {
    //     // speed 45%
    //     RefreshSpeedGauge(PercentToScale(0.75f));
    // }
    // public void test4()
    // {
    //     // penalty on
    //     TogglePenalty(true);
    // }
    // public void test5()
    // {
    //     // penalty off
    //     TogglePenalty(false);
    // }
    // public void test6()
    // {
    //     // speed 25%
    //     RefreshSpeedGauge(PercentToScale(0.25f));
    // }
    // public void test7()
    // {
    //     // speed 120%
    //     RefreshSpeedGauge(PercentToScale(1.2f));
    // }
    // public void test8()
    // {
    //     // Buff on
    //     ToggleBuff(true);
    // }
    // public void test9()
    // {
    //     // buff off
    //     ToggleBuff(false);
    // }

    // public void endTest()
    // {
    //     EndingSceneDataHolder.endingSceneInfos.SetInfos(6600, 6500, 13000, SceneManager.GetActiveScene().name);
    //     SceneManager.LoadScene("EndingScene");
    // }
}
