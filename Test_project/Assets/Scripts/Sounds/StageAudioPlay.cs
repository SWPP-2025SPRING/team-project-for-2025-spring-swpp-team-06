using UnityEngine;

public class StageAudioPlay : MonoBehaviour
{
    public AudioClip stageBgm;   // 인스펙터에 전투곡 할당
    // public float fadeTime = 1.0f; // 부드러운 페이드(초)

    void Start()
    {
        // AudioPlay 싱글턴이 살아 있는지 확인
        var audio = FindObjectOfType<AudioPlay>(); // 혹은 AudioPlay.Instance 프로퍼티

        if (audio == null || stageBgm == null) return;

        // 이미 같은 클립이면 무시
        if (audio.GetComponent<AudioSource>().clip == stageBgm) return;

        // AudioPlay에 크로스페이드 메서드를 추가해 두었다면:
        // audio.PlayBGM(stage1Bgm, fadeTime);

        // 간단히 끊고 바로 틀고 싶다면:
        var src = audio.GetComponent<AudioSource>();
        src.Stop();
        src.clip = stageBgm;
        src.Play();
    }
}