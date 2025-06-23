using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioPlay : MonoBehaviour
{
    /* ---------- 싱글턴 영역 ---------- */
    public static AudioPlay Instance { get; private set; }

    private AudioSource bgmSource;   // 기존 BGM 재생 전용
    private AudioSource sfxSource;   // 새로 만들 SFX 전용

    public AudioMixer gameMixer;     // 마스터 믹서
    public AudioMixerGroup bgmGroup; // “Music” 그룹 등
    public AudioMixerGroup sfxGroup; // “SFX”  그룹 등


    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // BGM용 AudioSource
        bgmSource = GetComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = bgmGroup;

        // SFX용 AudioSource (Run-time에 동적으로 추가)
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = sfxGroup;
        sfxSource.playOnAwake = false;
    }

    /* ---------- BGM 교체 기능 ---------- */
    public void PlayBGM(AudioClip nextClip, float fadeTime = 0f)
    {
        if (bgmSource.clip == nextClip) return;   // 이미 그 곡이면 무시

        if (fadeTime <= 0f)
        {
            bgmSource.clip = nextClip;
            bgmSource.Play();
        }
        else
        {
            StartCoroutine(FadeAndSwap(nextClip, fadeTime));
        }
    }

    /* ---------- SFX ---------- */
    public void PlaySFX(AudioClip clip, float volume = 1f, bool spatial = false, Vector3? pos = null)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, volume);
    }

    // 필요하면 Stop, Resume 도 그대로 둡니다
    public void StopBGM() => bgmSource.Stop();

    /* (선택) 외부에서 편하게 접근할 정적 래퍼 */
    public static void PlayBGMStatic(AudioClip clip, float fade = 0f) =>
        Instance?.PlayBGM(clip, fade);

    public static void PlaySFXStatic(AudioClip clip, float volume = 1f,
                                     bool spatial = false, Vector3? pos = null) =>
        Instance?.PlaySFX(clip, volume, spatial, pos);

    /* ---------- 내부 코루틴 ---------- */
    private System.Collections.IEnumerator FadeAndSwap(AudioClip next, float t)
    {
        for (float a = 0; a < t; a += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(1f, 0f, a / t);
            yield return null;
        }
        bgmSource.clip = next;
        bgmSource.Play();
        for (float a = 0; a < t; a += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0f, 1f, a / t);
            yield return null;
        }
    }
}
