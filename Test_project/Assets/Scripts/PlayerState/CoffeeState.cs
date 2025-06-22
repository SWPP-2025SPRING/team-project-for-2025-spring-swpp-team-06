using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeState : IPlayerState, IMovementModifier, IRemovable
{
    private float duration = 5f;
    private float timer = 0f;

    public bool ShouldRemove { get; private set; } = false;
    private InGameUIControl uiScript;

    public void Enter(PlayerControl player)
    {
        timer = 0f;
        ShouldRemove = false;

        GameObject uiObject = GameObject.FindWithTag("UI");
        if (uiObject != null)
        {
            uiScript = uiObject.GetComponent<InGameUIControl>();
            uiScript?.ToggleBuff(true);
        }

        Debug.Assert(uiScript != null);

        uiScript.InitiateGauge(duration, uiScript.coffeeTimeGauge);
    }

    public void Exit(PlayerControl player)
    {
        uiScript?.ToggleBuff(false);
        uiScript?.TurnOffGauge(uiScript.coffeeTimeGauge);
    }

    public void FixedUpdate(PlayerControl player)
    {
        timer += Time.fixedDeltaTime;
        if (timer >= duration)
        {
            ShouldRemove = true;
        }
    }

    public Type TypeOf()
    {
        return Type.Coffee;
    }

    public float GetAccelerationFactor()
    {
        return 1.5f;
    }

    public float GetMaxSpeedFactor()
    {
        return 1.25f;
    }

    public void ResetTimer()
    {
        timer = 0f;
        uiScript.InitiateGauge(duration, uiScript.coffeeTimeGauge);
    }
}
