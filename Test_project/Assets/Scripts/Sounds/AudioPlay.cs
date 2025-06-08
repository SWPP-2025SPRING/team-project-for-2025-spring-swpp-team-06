using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlay : MonoBehaviour
{
    /* ---------- 싱글턴 영역 ---------- */
    public static AudioPlay Instance { get; private set; }

    private AudioSource audioSource;

    void Awake ()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    /* ---------- BGM 교체 기능 ---------- */
    public void PlayBGM(AudioClip nextClip, float fadeTime = 0f)
    {
        if (audioSource.clip == nextClip) return;   // 이미 그 곡이면 무시

        if (fadeTime <= 0f)
        {
            audioSource.clip = nextClip;
            audioSource.Play();
        }
        else
        {
            StartCoroutine(FadeAndSwap(nextClip, fadeTime));
        }
    }

    // 필요하면 Stop, Resume 도 그대로 둡니다
    public void StopBGM() => audioSource.Stop();

    /* (선택) 외부에서 편하게 접근할 정적 래퍼 */
    public static void PlayBGMStatic(AudioClip clip, float fade = 0f) =>
        Instance?.PlayBGM(clip, fade);

    /* ---------- 내부 코루틴 ---------- */
    private System.Collections.IEnumerator FadeAndSwap(AudioClip next, float t)
    {
        for (float a = 0; a < t; a += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(1f, 0f, a / t);
            yield return null;
        }
        audioSource.clip = next;
        audioSource.Play();
        for (float a = 0; a < t; a += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, a / t);
            yield return null;
        }
    }
}
