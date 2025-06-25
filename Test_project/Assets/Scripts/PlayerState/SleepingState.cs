using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingState : IPlayerState, IMovementModifier, IRemovable
{
    private float timer = 0f;
    private float sleepDuration = 3f;
    public bool ShouldRemove { get; private set; } = false;
    private GameObject effectInstance;
    private float effectInterval = 1.5f;
    private float effectTimer;
    private InGameUIControl uiScript;

    public void Enter(PlayerControl player)
    {
        timer = 0f;
        effectTimer = 0f;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.groundRotator.enabled = false;
        ShouldRemove = false;

        PlayEffect(player);

        GameObject uiObject = GameObject.FindWithTag("UI");
        if (uiObject != null)
        {
            uiScript = uiObject.GetComponent<InGameUIControl>();
        }

        Debug.Assert(uiScript != null);

        uiScript.InitiateGauge(sleepDuration, uiScript.bedTimeGauge);

    }

    public void Exit(PlayerControl player)
    {
        player.groundRotator.enabled = true;
        if (effectInstance != null)
        {
            GameObject.Destroy(effectInstance);
        }
        Debug.Assert(uiScript != null);

        uiScript.TurnOffGauge(uiScript.bedTimeGauge);
    }

    public void FixedUpdate(PlayerControl player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        timer += Time.fixedDeltaTime;
        effectTimer += Time.fixedDeltaTime;

        if (timer >= sleepDuration)
        {
            ShouldRemove = true;
        }

        if (effectTimer >= effectInterval)
        {
            effectTimer = 0f;
            PlayEffect(player);
        }
    }

    public Type TypeOf()
    {
        return Type.FullPenalty;
    }

    public float GetAccelerationFactor()
    {
        return 0f;
    }

    public float GetMaxSpeedFactor()
    {
        return 0f;
    }
    private void PlayEffect(PlayerControl player)
    {
        GameObject sleepingEffectPrefab = Resources.Load<GameObject>("Effects/SleepingEffect");
        if (sleepingEffectPrefab != null)
        {
            effectInstance = GameObject.Instantiate(sleepingEffectPrefab, player.transform);
            effectInstance.transform.localPosition = Vector3.up * 10f;
            GameObject.Destroy(effectInstance, 1.5f);
        }
    }

    public void ResetTimer()
    {
        timer = 0f;
        uiScript.InitiateGauge(sleepDuration, uiScript.bedTimeGauge);
    }
}
