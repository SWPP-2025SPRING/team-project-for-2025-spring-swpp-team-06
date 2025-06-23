using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedObstacle : MonoBehaviour
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

    private HashSet<PlayerControl> playersInBed = new HashSet<PlayerControl>();
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        triggered = true;

        var hitClip = GetClip(clipPath);
        // SFX
        AudioPlay.PlaySFXStatic(hitClip, hitVolume, spatial: true, pos: transform.position);

        // Destroy once triggered
        Transform parent = transform.parent;
        if (parent.gameObject.CompareTag("Ground")) return; // If it is directly in ground

        foreach (Transform child in parent)
        {
            // Destroy all objects in same group
            Destroy(child.gameObject);
        }

        PlayerControl player = other.GetComponent<PlayerControl>();
        if (player == null) return;

        if (playersInBed.Contains(player)) return;

        foreach (var state in player.States)
        {
            if (state is SleepingState)
                return;
        }

        playersInBed.Add(player);
        player.PushState(new SleepingState());
    }
    private void OnTriggerStay(Collider other)
    {
        if (triggered) return;
        triggered = true;
        Debug.Log(transform.position.z);
        // Destroy once triggered
        Transform parent = transform.parent;
        if (parent.gameObject.CompareTag("Ground")) return; // If it is directly in ground

        foreach (Transform child in parent)
        {
            // Destroy all objects in same group
            Destroy(child.gameObject);
        }

        PlayerControl player = other.GetComponent<PlayerControl>();
        if (player == null) return;

        if (playersInBed.Contains(player)) return;

        foreach (var state in player.States)
        {
            if (state is SleepingState)
                return;
        }

        playersInBed.Add(player);
        player.PushState(new SleepingState());
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        if (player == null) return;

        if (playersInBed.Contains(player))
        {
            playersInBed.Remove(player);
        }

        
    }
}
