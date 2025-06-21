using UnityEngine;

public class TitleAudioPlay : MonoBehaviour
{

    //Modified By ChagGPT
    public AudioClip stageBgm; // 인스펙터에서 할당

    void Start()
    {
        var audio = AudioPlay.Instance; // AudioPlay 싱글턴 사용
        if (audio == null || stageBgm == null) return;

        var src = audio.GetComponent<AudioSource>();

        // 오디오가 재생 중이면
        if (src.isPlaying)
        {
            // 같은 클립이면 무시
            if (src.clip == stageBgm)
            {
                return;
            }

            // 다른 클립이면 Stop 후 재생
            src.Stop();
            src.clip = stageBgm;
            src.Play();
        }
        else
        {
            // 현재 멈춰 있는 경우 → 무조건 stageBgm 재생
            src.clip = stageBgm;
            src.Play();
        }
    }
}
