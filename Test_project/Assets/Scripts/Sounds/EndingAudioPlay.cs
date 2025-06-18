using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingAudioPlay : MonoBehaviour
{
    [Header("Ending BGM")]
    public AudioClip endingBgm;
    
    void Start()
    {
        // AudioPlay 싱글턴이 살아 있는지 확인
        var audio = FindObjectOfType<AudioPlay>(); // 혹은 AudioPlay.Instance 프로퍼티

        if (audio == null || endingBgm == null) return;

        // 이미 같은 클립이면 무시
        if (audio.GetComponent<AudioSource>().clip == endingBgm) return;

        var src = audio.GetComponent<AudioSource>();
        src.Stop();
        src.clip = endingBgm;
        src.Play();
    }
}