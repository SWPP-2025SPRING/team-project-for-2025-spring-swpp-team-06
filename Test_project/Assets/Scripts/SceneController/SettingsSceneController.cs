using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SettingsSceneController : MonoBehaviour
{

    public Slider sliderVolume;
    public AudioMixer gameMixer;
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

    /* 실제 볼륨 적용 + 필요 시 저장 */
    public void ApplyVolume(float dB, bool save)
    {
        gameMixer.SetFloat("MasterVol", dB);
        // gameMixer.SetFloat("SFXVol", dB);

        // BGM 전용 싱글턴 AudioPlay 볼륨도 맞춰 주고 싶다면 ↓
        // if (AudioPlay.Instance)
        //     AudioPlay.Instance.GetComponent<AudioSource>().volume = DbToLinear(dB);

        float lin = DbToLinear(dB);
        lin = Mathf.Clamp(lin, 0f, 1f);

        if (save)
            PlayerPrefs.SetFloat("Volume", lin);
    }

    /* 유틸: 선형(0‒1) ↔ dB 변환 -------------------------- */
    public static float LinearToDb(float lin) => Mathf.Log10(Mathf.Clamp(lin, 0.0001f, 1f)) * 20f;
    public static float DbToLinear(float dB) => Mathf.Pow(10f, dB / 20f);

    public void OnClickBackButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.UnloadSceneAsync("SettingsScene");
    }

    public void ChangeVolume()
    {
        float linear = sliderVolume.value;
        ApplyVolume(LinearToDb(linear), true);
    }

}
