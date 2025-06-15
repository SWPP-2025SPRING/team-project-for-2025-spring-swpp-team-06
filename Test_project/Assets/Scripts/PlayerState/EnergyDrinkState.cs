using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrinkState : IPlayerState, IRemovable
{
    private float duration = 3f;
    private float timer = 0f;
    private GameObject effectInstance;
    public bool ShouldRemove { get; private set; } = false;

    private InGameUIControl uiScript;

    public void Enter(PlayerControl player)
    {
        timer = 0f;
        ShouldRemove = false;
        if (player.HasState<DrunkenState>())
        {
            player.RemoveState(player.GetState<DrunkenState>());
        }

        if (player.HasState<SleepingState>())
        {
            player.RemoveState(player.GetState<SleepingState>());
        }

        if (player.HasState<PlayerGamingState>())
        {
            player.RemoveState(player.GetState<PlayerGamingState>());
        }

        GameObject energyDrinkEffectPrefab = Resources.Load<GameObject>("Effects/EnergyDrinkEffect");
        if (energyDrinkEffectPrefab != null)
        {
            effectInstance = GameObject.Instantiate(energyDrinkEffectPrefab, player.transform);
        }

        GameObject uiObject = GameObject.FindWithTag("UI");
        if (uiObject != null)
        {
            uiScript = uiObject.GetComponent<InGameUIControl>();
        }

        Debug.Assert(uiScript != null);

        uiScript.TurnOnGauge(uiScript.energyTimeGauge);

        uiScript.InitiateGauge(duration, uiScript.energyTimeGauge);
    }

    public void Exit(PlayerControl player)
    {
        if (effectInstance != null)
        {
            GameObject.Destroy(effectInstance);
        }
        uiScript.TurnOffGauge(uiScript.energyTimeGauge);

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
        return true;
    }

    public bool IsPenalty()
    {
        return false;
    }
    public bool IsCoffee()
    {
        return false;
    }
    
    public void ResetTimer()
    {
        timer = 0f;
        uiScript.TurnOffGauge(uiScript.coffeeTimeGauge);
        uiScript.InitiateGauge(duration, uiScript.energyTimeGauge);
    }
    public bool IsSoju() { return false; }
}
