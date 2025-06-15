using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkenState : IPlayerState, IRemovable
{
    private float duration = 5f;
    private float timer;
    private GroundRotator groundRotator;
    private GameObject effectInstance;

    private InGameUIControl uiScript;

    public bool ShouldRemove { get; private set; } = false;

    public void Enter(PlayerControl player)
    {
        timer = 0f;
        groundRotator = player.groundRotator;
        if (groundRotator != null)
        {
            groundRotator.reverseInput = true;
        }
        ShouldRemove = false;
        GameObject drunkenEffectPrefab = Resources.Load<GameObject>("Effects/DrunkenEffect");
        if (drunkenEffectPrefab != null)
        {
            effectInstance = GameObject.Instantiate(drunkenEffectPrefab, player.transform);
            effectInstance.transform.localPosition = Vector3.up * 1.5f;
        }

        GameObject uiObject = GameObject.FindWithTag("UI");
        if (uiObject != null)
        {
            uiScript = uiObject.GetComponent<InGameUIControl>();
        }

        Debug.Assert(uiScript != null);

        uiScript.TurnOnGauge(uiScript.sojuTimeGauge);

        uiScript.InitiateGauge(duration, uiScript.sojuTimeGauge);
    }

    public void Exit(PlayerControl player)
    {
        if (groundRotator != null)
        {
            groundRotator.reverseInput = false;
        }
        if (effectInstance != null)
        {
            GameObject.Destroy(effectInstance);
        }

        uiScript.TurnOffGauge(uiScript.sojuTimeGauge);
    }

    public void Update(PlayerControl player)
    {
        
    }

    public void FixedUpdate(PlayerControl player) 
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            ShouldRemove = true;
        }
    }

    public bool IsBlocking()
    {
        return false;
    }

    public bool IsPenalty()
    {
        return true;
    }
    public bool IsCoffee()
    {
        return false;
    }

    public void ResetTimer()
    {
        timer = 0f;
        uiScript.InitiateGauge(duration, uiScript.sojuTimeGauge);
    }
    public bool IsSoju(){ return true; }

}
