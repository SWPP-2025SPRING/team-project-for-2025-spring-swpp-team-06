using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// public class SettingsSceneController : MonoBehaviour
// {

//     public Slider sliderVolume;
//     // Start is called before the first frame update
//     void Start()
//     {
//         if (PlayerPrefs.GetFloat("Volume", -1f) < 0)
//         {
//             // hasn't set volume yet.
//             PlayerPrefs.SetFloat("Volume", 0.5f);
//         }

//         sliderVolume.value = PlayerPrefs.GetFloat("Volume", 0.5f);
//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }

//     public void OnClickBackButton()
//     {
//         EventSystem.current.SetSelectedGameObject(null);
//         SceneManager.UnloadSceneAsync("SettingsScene");
//     }

//     public void ChangeVolume()
//     {
//         PlayerPrefs.SetFloat("Volume", sliderVolume.value);
//     }

// }


public class SettingsSceneController : MonoBehaviour
{
    [Header("UI")]
    public Slider sliderVolume;

    [Header("Audio")]
    public AudioMixer gameMixer;          // GameMixer drag
    private const string MASTER_PARAM = "MasterVol";

    void Start()
    {
        // ① 지금 Mixer에 적용돼 있는 실시간 값을 읽는다
        float currDb;
        if (!gameMixer.GetFloat(MASTER_PARAM, out currDb))
            currDb = -10f;                         // 파라미터가 없으면 0 dB

        // ② 슬라이더 눈금만 현재 값에 맞춘다 (콜백 NO)
        sliderVolume.SetValueWithoutNotify(DbToLinear(currDb));

        // ③ 이제부터 움직임 감지
        sliderVolume.onValueChanged.AddListener(OnSliderChange);
    }

    /* --------------------------------------------------------------------- */
    public void OnSliderChange(float linear)
    {
        float dB = LinearToDb(linear);
        ApplyVolume(dB, save: true);
    }
    /* --------------------------------------------------------------------- */

    /* 실제 볼륨 적용 + 필요 시 저장 */
    void ApplyVolume(float dB, bool save)
    {
        gameMixer.SetFloat(MASTER_PARAM, dB);

        // BGM 전용 싱글턴 AudioPlay 볼륨도 맞춰 주고 싶다면 ↓
        if (AudioPlay.Instance)
            AudioPlay.Instance.GetComponent<AudioSource>().volume = DbToLinear(dB);

        if (save)
            PlayerPrefs.SetFloat(MASTER_PARAM, dB);
    }

    /* 유틸: 선형(0‒1) ↔ dB 변환 -------------------------- */
    static float LinearToDb(float lin) => Mathf.Log10(Mathf.Clamp(lin, 0.0001f, 1f)) * 20f;
    static float DbToLinear(float dB) => Mathf.Pow(10f, dB / 20f);

    /* 기존 ‘뒤로가기’ 버튼 처리 ---------------------------- */
    public void OnClickBackButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.UnloadSceneAsync("SettingsScene");
    }
}
