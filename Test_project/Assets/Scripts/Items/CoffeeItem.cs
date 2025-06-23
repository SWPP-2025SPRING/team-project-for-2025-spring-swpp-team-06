using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeItem : MonoBehaviour
{
    [Header("사운드")]
    [SerializeField, Range(0f,1f)] private float hitVolume = 2f;

    [SerializeField] private string clipPath = "Sounds/item_sound";
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
            Destroy(child.gameObject);
        }
        PlayerControl player = other.GetComponent<PlayerControl>();
        foreach (var state in player.States)
        {
            if (state is CoffeeState existingGamingState)
            {
                existingGamingState.ResetTimer();
                return;
            }
        }

        player.PushState(new CoffeeState());
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
            Destroy(child.gameObject);
        }
        PlayerControl player = other.GetComponent<PlayerControl>();
        foreach (var state in player.States)
        {
            if (state is CoffeeState existingGamingState)
            {
                existingGamingState.ResetTimer();
                return;
            }
        }

        player.PushState(new CoffeeState());
    }
}
