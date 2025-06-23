using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamingMachineObstacle : MonoBehaviour
{
    
    [Header("사운드")]
    // [SerializeField] private AudioClip hitClip;   // Inspector에서 지정
    [SerializeField, Range(0f,1f)] private float hitVolume = 2f;

    [SerializeField] private string clipPath = "Sounds/obstacle_sound";
    private static readonly Dictionary<string, AudioClip> clipCache = new();

    private AudioClip GetClip(string path)
    {
        if (clipCache.TryGetValue(path, out var cached)) return cached;

        var clip = Resources.Load<AudioClip>(path);

        if (clip == null)
        {
            Debug.LogWarning($"[SojuObstacle] AudioClip not found: {path}");
            return null;
        }

        clipCache[path] = clip;   // 다음부터 빠르게
        return clip;
    }

    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        triggered = true;

        // SFX
        var hitClip = GetClip(clipPath);
        AudioPlay.PlaySFXStatic(hitClip, hitVolume, spatial: true, pos: transform.position);

        // Destroy once triggered
        Transform parent = transform.parent;
        if (parent.gameObject.CompareTag("Ground")) return;

        foreach (Transform child in parent)
        {
            // Destroy all objects in same group
            Destroy(child.gameObject);
        }
        PlayerControl player = other.GetComponent<PlayerControl>();
        foreach (var state in player.States)
        {
            if (state is PlayerGamingState existingGamingState)
            {
                existingGamingState.ResetTimer();
                return;
            }
        }

        player.PushState(new PlayerGamingState());



    }
    private void OnTriggerStay(Collider other)
    {
        if (triggered) return;
        triggered = true;


        // Destroy once triggered
        Transform parent = transform.parent;
        if (parent.gameObject.CompareTag("Ground")) return;

        foreach (Transform child in parent)
        {
            // Destroy all objects in same group
            Destroy(child.gameObject);
        }
        PlayerControl player = other.GetComponent<PlayerControl>();
        foreach (var state in player.States)
        {
            if (state is PlayerGamingState existingGamingState)
            {
                existingGamingState.ResetTimer();
                return;
            }
        }

        player.PushState(new PlayerGamingState());



    }
}
